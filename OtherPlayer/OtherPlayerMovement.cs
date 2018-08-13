using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Assets.Scripts.WebSocket.Models;
using UnityEngine;

public class OtherPlayerMovement : MonoBehaviour {

    public float Speed = 6f;

    private Vector3 _movement;
    private Animator _anim;
    private Rigidbody _playerRigidbody;

    private int _floorMask;
    private float _camaRayLength = 100f;
    
    public ConcurrentQueue<ServerMovement> ServerMovements = new ConcurrentQueue<ServerMovement>(); 

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
        _anim = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        ServerMovements = new ConcurrentQueue<ServerMovement>();
    }

    private void FixedUpdate()
    {
        ServerMovement serverMovement;
        if (!ServerMovements.TryDequeue(out serverMovement))
            return;

        float h = serverMovement.Horizontal;
        float v = serverMovement.Vertical;
        Vector3 mousePosition = new Vector3(serverMovement.MousePositionX, serverMovement.MousePositionY, serverMovement.MousePositionZ);
        Move(h, v);
        Turning(mousePosition);
        Animating(h, v);
    }

    private void Move(float h, float v)
    {
        _movement.Set(h, 0f, v);
        _movement = _movement.normalized * Speed * Time.deltaTime;
        var newPosition = transform.position + _movement;
        _playerRigidbody.MovePosition(newPosition);
    }

    private void Turning(Vector3 mousePosition)
    {
        Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, _camaRayLength, _floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            _playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        _anim.SetBool("IsWalking", walking);
    }
}
