using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 playerDirection;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text coinSummaryText;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip startGameSound;
    public AudioClip coinSound;
    public AudioClip virusSound;
    public AudioClip buttonClickSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (audioSource != null && startGameSound != null)
            audioSource.PlayOneShot(startGameSound);
    }

    private void Update()
    {
        float inputY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, inputY).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = playerDirection * playerSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Virus"))
        {
            int coins = CoinManager.GetCoins();
            PlayerPrefs.SetInt("coins_collected", coins);
            PlayerPrefs.SetInt("earned_pet_coins", coins * 10);
            PlayerPrefs.Save();
            
            if (virusSound != null && audioSource != null)
                audioSource.PlayOneShot(virusSound);

            rb.linearVelocity = Vector2.zero; 
            Time.timeScale = 0f;        
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (coinSummaryText != null)
                    coinSummaryText.text = $"COINS {coins * 10} ";
            }
        }
        else if (other.CompareTag("Coin"))
        {
            if (coinSound != null && audioSource != null)
                audioSource.PlayOneShot(coinSound); 

            CoinManager.AddCoin();
            Destroy(other.gameObject);
        }
    }

    
    public void BackToPetScene()
    {
        if (buttonClickSound != null && audioSource != null)
            audioSource.PlayOneShot(buttonClickSound);

        Time.timeScale = 1f;
        SceneManager.LoadScene("PetScene");
    }
}