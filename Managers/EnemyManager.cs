using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth PlayerHealth;
    public GameObject Enemy;
    public float SpawnTime = 3f;
    public Transform[] SpawnPoints;
    
    void Start ()
    {
        InvokeRepeating ("Spawn", SpawnTime, SpawnTime);
    }


    void Spawn ()
    {
        if(PlayerHealth.CurrentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, SpawnPoints.Length);

        Instantiate (Enemy, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
    }
}
