using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject[] targets; // 0=Coin, 1=Clock, 2=BlackDog
    public float[] spawnWeights = { 0.85f, 0.03f, 0.12f }; // Coin=60%, Clock=20%, Dog=20%
    public float spawnInterval = 1.5f;
    public int maxTargets = 5; // จำนวนเป้าสูงสุดในฉาก
    public float xMin = -8f;
    public float xMax = 8f;
    public float yMin = 0f;
    public float yMax = 4.5f;


    void Start()
    {
        InvokeRepeating("SpawnTarget", 1f, spawnInterval);
    }

    int GetWeightedRandomIndex()
    {
        float total = 0f;
        foreach (float weight in spawnWeights)
            total += weight;

        float randomValue = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < spawnWeights.Length; i++)
        {
            cumulative += spawnWeights[i];
            if (randomValue <= cumulative)
                return i;
        }

        return spawnWeights.Length - 1; // fallback ป้องกัน error
    }

    void SpawnTarget()
    {
        int currentTargetCount = GameObject.FindGameObjectsWithTag("Coin").Length
            + GameObject.FindGameObjectsWithTag("Clock").Length
            + GameObject.FindGameObjectsWithTag("BlackDog").Length;

        if (currentTargetCount >= maxTargets) return;

        int index = GetWeightedRandomIndex();

        // ไม่ให้ Clock spawn ถ้าใช้ครบ 5 ครั้ง
        if (targets[index].tag == "Clock")
        {
            if (GameManager.clockUsed >= GameManager.maxClockUse)
            {
                return; // หยุด ไม่ spawn Clock
            }

            // ยัง spawn ได้ แต่ห้ามมีหลายอันพร้อมกัน
            if (GameObject.FindGameObjectsWithTag("Clock").Length >= 1)
            {
                return;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);
            Vector3 spawnPos = new Vector3(x, y, 0);

            if (Physics2D.OverlapCircle(spawnPos, 0.5f) == null)
            {
                Instantiate(targets[index], spawnPos, Quaternion.identity);
                break;
            }
        }
    }
}
