using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WebSocket;
using UnityEngine;
using WebSocket;

public class SocketManager : MonoBehaviour {


    // Use this for initialization
    void Start ()
    {

        UnitySystemConsoleRedirector.Redirect();
        //   EventChannel.Connect();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
