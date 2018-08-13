using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class EnemyAttack : MonoBehaviour
{
    public float TimeBetweenAttacks = 0.5f;
    public int AttackDamage = 10;


    private Animator _anim;
    private GameObject _player;
    private PlayerHealth _playerHealth;
    private EnemyHealth _enemyHealth;
    private bool _playerInRange;
    private float _timer;

    private GameObject _otherPlayer;
    private GameObject _playerBeingAttacked;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _anim = GetComponent<Animator>();
        _otherPlayer = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_otherPlayer == null)
            _otherPlayer = GameObject.FindGameObjectWithTag("OtherPlayer");
        if (other.gameObject == _player)
        {
            _playerBeingAttacked = _player;
            _playerInRange = true;
        }
        else if (_otherPlayer != null && other.gameObject == _otherPlayer)
        {
            _playerBeingAttacked = _otherPlayer;
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_otherPlayer == null)
            _otherPlayer = GameObject.FindGameObjectWithTag("OtherPlayer");

        if (other.gameObject == _player)
        {
            _playerBeingAttacked = null;
            _playerInRange = false;
        }
        else if (_otherPlayer != null && other.gameObject == _otherPlayer)
        {
            _playerBeingAttacked = null;
            _playerInRange = false;
        }
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= TimeBetweenAttacks && _playerInRange && _enemyHealth.CurrentHealth > 0)
        {
            Attack();
        }

        if (_playerHealth.CurrentHealth <= 0)
        {
            _anim.SetTrigger("PlayerDead");
        }
    }


    private void Attack()
    {
        _timer = 0f;

        var playerHealth = _playerBeingAttacked.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.CurrentHealth > 0)
            playerHealth.TakeDamage(AttackDamage);

        var otherPlayerHealth = _playerBeingAttacked.GetComponent<OtherPlayerHealth>();
        if (otherPlayerHealth != null && otherPlayerHealth.CurrentHealth > 0)
            otherPlayerHealth.TakeDamage(AttackDamage);
    }
}
