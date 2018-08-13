using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Assets.Scripts.WebSocket.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject OtherPlayer;

    private ServerGame m_currenGame;
    
    public CameraFollow CameraFollow;

    // Use this for initialization
    void Start () {
        
        Console.WriteLine("Hide");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void WebSocketOnOnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        var message = messageEventArgs.Data;
        var messageContent = ServerMessage.GetMessageContent(message);
        ServerGame game;
        switch (ServerMessage.GetMessageType(message))
        {
            case ServerMessage.PlayerSpawnType:
                game = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                GameStartUiManager.RoomId = game.RoomId;
                UnityMainThreadDispatcher.Instance().Enqueue(() => SpawnPlayer(game));
                break;
            case ServerMessage.PlayerPositioningType:
                game = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                Console.WriteLine(game);
                UnityMainThreadDispatcher.Instance().Enqueue(() => RelocatePlayer(game));
                break;
            case ServerMessage.PlayerMoveType:
                game = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                UnityMainThreadDispatcher.Instance().Enqueue(() => MovePlayer(game));
                break;
            case ServerMessage.PlayerShootType:
                game = JsonConvert.DeserializeObject<ServerGame>(messageContent, settings);
                UnityMainThreadDispatcher.Instance().Enqueue(() => ShootPlayer(game));
                break;
            default:
                break;
        }
    }

    public void SpawnPlayer(ServerGame game)
    {
        var queueButton = GameObject.FindGameObjectWithTag("QueueButton").GetComponent<Button>();
        Time.timeScale = GameStartUiManager.OriginalTimeScalse;
        queueButton.gameObject.SetActive(false);

        m_currenGame = game;
        var controllerPlayer = game.Player1.PlayerId == game.ControllerPlayerId ? game.Player1 : game.Player2;
        var otherPlayer = game.Player1.PlayerId == game.ControllerPlayerId ? game.Player2 : game.Player1;

        var spawnPointController = new Vector3(controllerPlayer.PositionX, 0, controllerPlayer.PositionZ);
        var spawnPointOtherPlayer = new Vector3(otherPlayer.PositionX, 0, otherPlayer.PositionZ);
        var rotation = new Quaternion(0, 0, 0, 0);

        Player.transform.position = spawnPointController;
        Console.WriteLine("Spawned");
        GameStartUiManager.DisableShoot = false;


        Instantiate(OtherPlayer, spawnPointOtherPlayer, rotation);
        OtherPlayer = GameObject.FindGameObjectWithTag("OtherPlayer");
        var healthSlider = GameObject.FindGameObjectWithTag("Player1HealthSlider").GetComponent<Slider>();
        OtherPlayer.GetComponent<OtherPlayerHealth>().HealthSlider = healthSlider;
        Console.WriteLine(OtherPlayer.GetComponent<OtherPlayerHealth>().HealthSlider.maxValue);
    }

    public void RelocatePlayer(ServerGame game)
    {
        if (OtherPlayer == null)
            return;

        var otherPlayer = game.GetOtherPlayer();

        //var controllerPlayer = game.GetControllerPlayer();
        //Console.WriteLine("Position");
        //Console.WriteLine(controllerPlayer.PositionX);
        Console.WriteLine(otherPlayer.PositionX);
        OtherPlayer.transform.position = new Vector3(otherPlayer.PositionX, otherPlayer.PositionY, otherPlayer.PositionZ);
    }

    public void MovePlayer(ServerGame game)
    {
        if (OtherPlayer == null)
            return;

        var otherPlayer = game.GetOtherPlayer();
        if (otherPlayer.Movement == null)
            return;

        var movements = OtherPlayer.GetComponent<OtherPlayerMovement>();
        movements.ServerMovements.Enqueue(otherPlayer.Movement);
    }

    public void ShootPlayer(ServerGame game)
    {
        if (OtherPlayer == null)
            return;

        var otherPlayer = game.GetOtherPlayer();
        if (!otherPlayer.IsShooting)
            return;

        var movements = OtherPlayer.GetComponentInChildren<OtherPlayerShooting>();
        movements.ShootingQueue.Enqueue(true);
    }

    public void SetEventMessages(WebSocketSharp.WebSocket webSocket)
    {
        webSocket.OnMessage += WebSocketOnOnMessage;
    }
}
