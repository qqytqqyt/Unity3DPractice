using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WebSocket;
using Assets.Scripts.WebSocket.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using WebSocket;
using WebSocketSharp;
using Random = System.Random;

public class GameStartUiManager : MonoBehaviour
{
    public static bool DisableShoot = false;

    public static float OriginalTimeScalse = 0;

    public PlayerManager PlayerManager;
    
    public static EventChannel EventChannel = new EventChannel();

    public Button QueueButton;

    public static bool Queued;

    public static int PlayerId = 0;

    public static int RoomId = 0;

	// Use this for initialization
    private void Start ()
    {
        
        UnitySystemConsoleRedirector.Redirect();
        OriginalTimeScalse = Time.timeScale;
        Time.timeScale = 0;
        Queued = false;
        PlayerId = (new Random()).Next(1000);
        Console.WriteLine("Your playerId is " + PlayerId);
        QueueButton = GameObject.FindGameObjectWithTag("QueueButton").GetComponent<Button>();
        EventChannel.Connect();
        EventChannel.WebSocket.OnMessage += WebSocketOnOnMessage;
        Console.WriteLine("Created");
    }
	
	// Update is called once per frame
    private void Update () {
		
	}

    public void Click()
    {
        if (Queued)
            return;

        Queued = true;
        EventChannel.Queue(PlayerId);
        Debug.Log("Queued...");
        QueueButton.GetComponentInChildren<Text>().text = "Queueing...";
        QueueButton.interactable = false;
    }

    private void WebSocketOnOnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        var message = messageEventArgs.Data;
        var messageContent = ServerMessage.GetMessageContent(message);
        switch (ServerMessage.GetMessageType(message))
        {
            case ServerMessage.PlayerSpawnType:
                var game = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                RoomId = game.RoomId;
                UnityMainThreadDispatcher.Instance().Enqueue(() => StartGame(game));
                break;
            case ServerMessage.PlayerPositioningType:
                var serverGame = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                Console.WriteLine(serverGame);
                UnityMainThreadDispatcher.Instance().Enqueue(() => PlayerManager.RelocatePlayer(serverGame));
                break;
            default:
                break;
        }
    }

    private void StartGame(ServerGame game)
    {
        Time.timeScale = OriginalTimeScalse;
        QueueButton.gameObject.SetActive(false);
        PlayerManager.SpawnPlayer(game);
    }


    public void PointerEnter()
    {
        DisableShoot = true;
    }

    public void PointerExit()
    {
        DisableShoot = false;
    }
}
