using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return; // ห้ามยิงโดนตัวเอง

        GameManager.instance.HandleTargetHit(other.gameObject);
        Destroy(gameObject);
    }
}