using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth PlayerHealth;
    public float RestartDelay = 5f;


    private Animator _anim;
    private float _restartTimer;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (PlayerHealth != null && PlayerHealth.CurrentHealth <= 0)
        {
            _anim.SetTrigger("GameOver");

            _restartTimer += Time.deltaTime;

            if (_restartTimer >= RestartDelay)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
