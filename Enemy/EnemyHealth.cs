using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int StartingHealth = 100;
    public int CurrentHealth;
    public float SinkSpeed = 2.5f;
    public int ScoreValue = 10;
    public AudioClip DeathClip;


    private Animator _anim;
    private AudioSource _enemyAudio;
    private ParticleSystem _hitParticles;
    private CapsuleCollider _capsuleCollider;
    private bool _isDead;
    private bool _isSinking;


    private void Awake ()
    {
        _anim = GetComponent <Animator> ();
        _enemyAudio = GetComponent <AudioSource> ();
        _hitParticles = GetComponentInChildren <ParticleSystem> ();
        _capsuleCollider = GetComponent <CapsuleCollider> ();

        CurrentHealth = StartingHealth;
    }


    private void Update ()
    {
        if(_isSinking)
        {
            transform.Translate (-Vector3.up * SinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(_isDead)
            return;

        _enemyAudio.Play ();

        CurrentHealth -= amount;
            
        _hitParticles.transform.position = hitPoint;
        _hitParticles.Play();

        if(CurrentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        _isDead = true;

        _capsuleCollider.isTrigger = true;

        _anim.SetTrigger ("Dead");

        _enemyAudio.clip = DeathClip;
        _enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        _isSinking = true;
        ScoreManager.Score += ScoreValue;
        Destroy (gameObject, 2f);
    }
}
