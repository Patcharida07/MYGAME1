using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pet"))
        {
            // แสดงผลหรือเล่น animation ได้
            Destroy(gameObject);
        }
    }
}