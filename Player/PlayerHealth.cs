using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth = 100;
    public int CurrentHealth;
    public Slider HealthSlider;
    public Image DamageImage;
    public AudioClip DeathClip;
    public float FlashSpeed = 5f;
    public Color FlashColour = new Color(1f, 0f, 0f, 0.1f);


    private Animator _anim;
    private AudioSource _playerAudio;
    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;
    private bool _isDead;
    private bool _damaged;


    private void Awake ()
    {
        _anim = GetComponent <Animator> ();
        _playerAudio = GetComponent <AudioSource> ();
        _playerMovement = GetComponent <PlayerMovement> ();
        _playerShooting = GetComponentInChildren <PlayerShooting> ();
        CurrentHealth = StartingHealth;
    }


    private void Update ()
    {
        if(_damaged)
        {
            if (DamageImage != null)
                DamageImage.color = FlashColour;
        }
        else
        {
            if (DamageImage != null)
                DamageImage.color = Color.Lerp (DamageImage.color, Color.clear, FlashSpeed * Time.deltaTime);
        }
        _damaged = false;
    }


    public void TakeDamage (int amount)
    {
        _damaged = true;

        CurrentHealth -= amount;

        HealthSlider.value = CurrentHealth;

        _playerAudio.Play ();

        if(CurrentHealth <= 0 && !_isDead)
        {
            Death ();
        }
    }


    private void Death ()
    {
        _isDead = true;

        _playerShooting.DisableEffects ();

        _anim.SetTrigger ("Die");

        _playerAudio.clip = DeathClip;
        _playerAudio.Play ();

        _playerMovement.enabled = false;
        _playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
