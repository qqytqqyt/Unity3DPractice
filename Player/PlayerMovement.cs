using System;
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

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    private void Move(float h, float v)
    {
        _movement.Set(h, 0f, v);
        _movement = _movement.normalized * Speed * Time.deltaTime;
        var newPosition = transform.position + _movement;
        _playerRigidbody.MovePosition(newPosition);

        GameStartUiManager.EventChannel.Move(GameStartUiManager.PlayerId, GameStartUiManager.RoomId, newPosition);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
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
