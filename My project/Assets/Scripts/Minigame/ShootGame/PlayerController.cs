using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }
}