﻿using System;
using Assets.Scripts.WebSocket;
using UnityEngine;
using WebSocket;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;

    private Vector3 _movement;
    private Animator _anim;
    private Rigidbody _playerRigidbody;

    private int _floorMask;
    private float _camaRayLength = 100f;

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
        _anim = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var mousePosition = Input.mousePosition;
        if (h != 0 || v != 0)
            GameStartUiManager.EventChannel.PlayerMove(GameStartUiManager.PlayerId, GameStartUiManager.RoomId, h, v, mousePosition);
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
