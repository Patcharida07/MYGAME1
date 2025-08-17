using UnityEngine;

public class ShowerController : MonoBehaviour
{
    public GameObject waterDropPrefab;
    public Transform dropPoint;

    public float dropRate = 0.5f;
    public float swingAngle = 30f;
    public float swingSpeed = 2f;

    private float dropTimer;

    void Update()
    {
        // แกว่งฝักบัวซ้าย-ขวาแบบ Sine wave
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ปล่อยน้ำหยดตามเวลา
        dropTimer += Time.deltaTime;
        if (dropTimer >= dropRate)
        {
            Instantiate(waterDropPrefab, dropPoint.position, Quaternion.identity);
            dropTimer = 0f;
        }
    }
}