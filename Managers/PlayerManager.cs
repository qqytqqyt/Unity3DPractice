using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WebSocket.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

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

    public void SpawnPlayer(ServerGame game)
    {
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
        OtherPlayer.GetComponent<PlayerHealth>().HealthSlider = healthSlider;
        Console.WriteLine(OtherPlayer.GetComponent<PlayerHealth>().HealthSlider.maxValue);
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
}
