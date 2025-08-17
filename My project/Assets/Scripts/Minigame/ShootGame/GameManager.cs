using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMP_Text scoreText, coinText, timeText;

    public float gameTime = 20f;
    private int score = 0;
    private int coins = 0;

    public static int clockUsed = 0;
    public static int maxClockUse = 5;

    [Header("Game Over UI")]
    public GameObject gameOverCanvas;     
    public TMP_Text gameOverCoinText;     
    public string petSceneName = "PetScene";

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip clockSound;
    public AudioClip badHitSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;
    public AudioSource bgmSource;
    public AudioClip introBGM;

    void Start()
    {
        Time.timeScale = 1f; 
        if (bgmSource != null && introBGM != null)
        {
            bgmSource.clip = introBGM;
            bgmSource.loop = false; 
            bgmSource.Play();
        }
    }
    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        gameOverCanvas.SetActive(false); 
    }

    void Update()
    {
        gameTime -= Time.deltaTime;
        timeText.text = ": " + Mathf.Ceil(gameTime).ToString();

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    public void HandleTargetHit(GameObject target)
    {
        switch (target.tag)
        {
            case "Money":
                score += 100;
                coins += 10;
                PlaySound(coinSound);
                break;
            case "Clock":
                if (clockUsed < maxClockUse)
                {
                    gameTime += 5f;
                    clockUsed++;
                    PlaySound(clockSound);
                }
                break;
            case "BlackDog":
                score -= 100;
                coins = Mathf.Max(0, coins - 10);
                PlaySound(badHitSound);
                break;
        }

        scoreText.text = "Score: " + score;
        coinText.text = ": " + coins;
        Destroy(target);
    }

    void EndGame()
    {
        gameTime = 0f;
        Time.timeScale = 0f;

       
        PlayerPrefs.SetInt("earned_pet_coins", coins);
        PlayerPrefs.Save();

        PlaySound(gameOverSound);

        gameOverCanvas.SetActive(true);
        if (gameOverCoinText != null)
            gameOverCoinText.text = "You earned " + coins + " coins!";
    }

    
    public void GoToPetScene()
    {
        PlaySound(buttonClickSound);
        Time.timeScale = 1f;
        SceneManager.LoadScene(petSceneName);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}