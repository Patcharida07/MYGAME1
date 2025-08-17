using UnityEngine;

public class AutoBubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float spawnInterval = 0.8f;
    public float spawnRadius = 1.5f;
    public Transform spawnCenter;

    public AudioSource audioSource;
    public AudioClip bubbleSound; 
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBubble();
            timer = 0f;
        }
    }

    void SpawnBubble()
    {
        Vector3 offset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, 0f);
        Vector3 spawnPos = spawnCenter.position + offset;

        Instantiate(bubblePrefab, spawnPos, Quaternion.identity);

        if (audioSource != null && bubbleSound != null)
        {
            audioSource.PlayOneShot(bubbleSound);
        }
    }
}