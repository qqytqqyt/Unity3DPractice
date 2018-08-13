using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerShooting : MonoBehaviour
{

    public int DamagePerShot = 20;
    public float TimeBetweenBullets = 0.15f;
    public float Range = 100f;


    private float _timer;
    private Ray _shootRay = new Ray();
    private RaycastHit _shootHit;
    private int _shootableMask;
    private ParticleSystem _gunParticles;
    private LineRenderer _gunLine;
    private AudioSource _gunAudio;
    private Light _gunLight;
    public readonly float _effectsDisplayTime = 0.2f;

    public ConcurrentQueue<bool> ShootingQueue = new ConcurrentQueue<bool>();

    private void Awake()
    {
        _shootableMask = LayerMask.GetMask("Shootable");
        _gunParticles = GetComponent<ParticleSystem>();
        _gunLine = GetComponent<LineRenderer>();
        _gunAudio = GetComponent<AudioSource>();
        _gunLight = GetComponent<Light>();
        ShootingQueue = new ConcurrentQueue<bool>();
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= TimeBetweenBullets * _effectsDisplayTime)
        {
            DisableEffects();
        }

        bool isShooting;
        if (ShootingQueue.TryDequeue(out isShooting))
        {
            Shoot();
        }

    }

    public void DisableEffects()
    {
        _gunLine.enabled = false;
        _gunLight.enabled = false;
    }

    void Shoot()
    {
        if (GameStartUiManager.DisableShoot)
            return;

        _timer = 0f;

        _gunAudio.Play();

        _gunLight.enabled = true;

        _gunParticles.Stop();
        _gunParticles.Play();

        _gunLine.enabled = true;
        _gunLine.SetPosition(0, transform.position);

        _shootRay.origin = transform.position;
        _shootRay.direction = transform.forward;

        if (Physics.Raycast(_shootRay, out _shootHit, Range, _shootableMask))
        {
            EnemyHealth enemyHealth = _shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(DamagePerShot, _shootHit.point);
            }
            _gunLine.SetPosition(1, _shootHit.point);
        }
        else
        {
            _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * Range);
        }
    }
}
