using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform _player;
    private PlayerHealth _playerHealth;
    private EnemyHealth _enemyHealth;
    private UnityEngine.AI.NavMeshAgent _nav;


    private void Awake ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").transform;
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _nav = GetComponent <NavMeshAgent> ();
    }


    private void Update ()
    {
        var otherPlayer = GameObject.FindGameObjectWithTag("OtherPlayer");
        
        if (_enemyHealth.CurrentHealth > 0 && _playerHealth.CurrentHealth > 0)
        {
            if (otherPlayer != null)
            {
                var toPlayer = Vector3.Distance(_player.position, transform.position);
                var toOtherPlayer = Vector3.Distance(otherPlayer.transform.position, transform.position);
                if (toPlayer > toOtherPlayer)
                    _nav.SetDestination(otherPlayer.transform.position);
                else
                    _nav.SetDestination(_player.position);
            }
            else
                _nav.SetDestination(_player.position);
        }
        else
        {
            _nav.enabled = false;
        }
    }
}
