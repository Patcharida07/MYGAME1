using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject coin;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float timeBetweenSpawn;
    public float coinSpawnChance = 0.3f;
    private float spawnTime;

   void Update()
    {
        if(Time.time > spawnTime)
        {
            Spawn();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0);

        if (Random.value < coinSpawnChance)
        {
            Instantiate(coin, spawnPos, Quaternion.identity);
            Debug.Log("Spawned coin at: " + spawnPos);
        }
        else
        {
            Instantiate(obstacle, spawnPos, transform.rotation);
            Debug.Log("Spawned obstacle at: " + spawnPos);
        }
    }
}



