using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PetController : MonoBehaviour
{
    public int love, hunger, clean;

    [Header("UI Elements")]
    public Slider loveBar, hungerBar, cleanBar;
    public TMP_Text timeText;
    public Image emotionIcon;
    public int petCoins = 0;
    public TMP_Text coinText;

    [Header("Emotion Sprites")]
    public Sprite happyIcon, sadIcon, hungryIcon, dirtyIcon;
    private PetMood currentMood = PetMood.Idle;
    private float moodStartTime = 0f;
    private float moodMinDuration = 10f;

    [Header("Animation")]
    public Animator animator;
    public enum PetMood { Idle = 0, Happy = 1, Sleep = 2, Angry = 3 }

    [Header("Feeding")]
    public FoodThrower foodThrower;

    private float timer;
    private float emotionCooldown = 0f; 

    [Header("Bubble System")]
    public GameObject emotionBubblePrefab; 
    public Transform bubbleSpawnPoint;
    public Canvas uiCanvas;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    [Header("Random Speech")]
    public string[] randomSpeechLines;
    public float randomSpeechIntervalMin = 5f;
    public float randomSpeechIntervalMax = 15f;

    private float lastEmotionShownTime = -10f;
    private Coroutine speechCoroutine;

    private Dictionary<PetMood, float> moodDurations = new Dictionary<PetMood, float>()
    {
        { PetMood.Idle, 0f },
        { PetMood.Happy, 10f },
        { PetMood.Angry, 10f },
        { PetMood.Sleep, 20f }
    };

    void Start()
    {
        Time.timeScale = 1f;
        love = PlayerPrefs.GetInt("love", 100);
        hunger = PlayerPrefs.GetInt("hunger", 100);
        clean = PlayerPrefs.GetInt("clean", 100);

        petCoins = PlayerPrefs.GetInt("petCoins", 0);

        if (PlayerPrefs.HasKey("earned_pet_coins"))
        {
            int earnedCoins = PlayerPrefs.GetInt("earned_pet_coins");
            petCoins += earnedCoins;
            PlayerPrefs.DeleteKey("earned_pet_coins");
            PlayerPrefs.SetInt("petCoins", petCoins);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("earned_clean"))
        {
            clean = Mathf.Min(clean + PlayerPrefs.GetInt("earned_clean"), 100);
            PlayerPrefs.DeleteKey("earned_clean");
        }

        if (PlayerPrefs.HasKey("earned_love"))
        {
            love = Mathf.Min(love + PlayerPrefs.GetInt("earned_love"), 100);
            PlayerPrefs.DeleteKey("earned_love");
        }

        if (PlayerPrefs.HasKey("next_mood"))
        {
            animator.SetInteger("Mood", PlayerPrefs.GetInt("next_mood"));
            PlayerPrefs.DeleteKey("next_mood");
        }

        UpdateUI();
        CheckForFood();
        StartRandomSpeechLoop();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10f)
        {
            hunger = Mathf.Max(hunger - 20, 0);
            clean = Mathf.Max(clean - 15, 0);
            love = Mathf.Max(love - 10, 0);

            SaveAll();
            UpdateUI();
            timer = 0;
        }

        timeText.text = System.DateTime.Now.ToString("HH:mm:ss");

        if (emotionCooldown <= 0)
        {
            UpdateEmotion();
            emotionCooldown = 5f;
        }
        else
        {
            emotionCooldown -= Time.deltaTime;
        }
    }

    void UpdateEmotion()
    {
        if (Time.time - lastEmotionShownTime < 3f) return;

        // Keep Sleep if still hungry
        if (currentMood == PetMood.Sleep && hunger < 30)
            return;

        float currentMinDuration = moodDurations.ContainsKey(currentMood) ? moodDurations[currentMood] : 10f;
        if (Time.time - moodStartTime < currentMinDuration)
            return;

        int minStat = Mathf.Min(hunger, clean, love);

        PetMood newMood = PetMood.Idle;
        string message = "";
        Sprite icon = null;

        if (minStat >= 30)
        {
            newMood = PetMood.Happy;
            string[] happyLines = { "やったー！", "うれしい！", "たのしい！" };
            message = happyLines[Random.Range(0, happyLines.Length)];
            icon = happyIcon;
        }
        else if (hunger == minStat)
        {
            newMood = PetMood.Sleep;
            string[] hungryLines = { "おなかすいたよ～", "なにかたべたいな", "おいしいものちょうだい" };
            message = hungryLines[Random.Range(0, hungryLines.Length)];
            icon = hungryIcon;
        }
        else if (clean == minStat || love == minStat)
        {
            newMood = PetMood.Angry;
            string[] dirtyLines = { "シャワーあびたいよ～", "あらいたいな", "さっぱりしたい" };
            message = dirtyLines[Random.Range(0, dirtyLines.Length)];
            icon = dirtyIcon;
        }

        if (newMood != currentMood)
        {
            currentMood = newMood;
            moodStartTime = Time.time;

            animator.SetInteger("Mood", (int)newMood);
            ShowEmotionBubble(message, icon);
            lastEmotionShownTime = Time.time;
        }
    }

    void ShowEmotionBubble(string message, Sprite icon)
    {
        if (emotionBubblePrefab == null || bubbleSpawnPoint == null || uiCanvas == null)
        {
            Debug.LogWarning("❌ Missing reference for bubble prefab / spawn point / canvas!");
            return;
        }

        GameObject bubble = Instantiate(emotionBubblePrefab, uiCanvas.transform);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(bubbleSpawnPoint.position);
        bubble.transform.position = screenPos;

        Text uiText = bubble.GetComponentInChildren<Text>();
        if (uiText != null)
            uiText.text = message;

        Transform emojiIconTransform = bubble.transform.Find("EmojiIcon");
        if (emojiIconTransform != null)
        {
            Image iconImage = emojiIconTransform.GetComponent<Image>();
            if (icon != null)
            {
                iconImage.sprite = icon;
                iconImage.enabled = true;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.enabled = false;
                iconImage.gameObject.SetActive(false);
            }
        }

        Destroy(bubble, 2.5f);
        lastEmotionShownTime = Time.time;
    }

    public void ShowEmotion(string message, Sprite icon)
    {
        if (Time.time - lastEmotionShownTime < 3f) return;
        ShowEmotionBubble(message, icon);
    }

    void StartRandomSpeechLoop()
    {
        if (speechCoroutine != null)
            StopCoroutine(speechCoroutine);

        speechCoroutine = StartCoroutine(RandomSpeechCoroutine());
    }

    IEnumerator RandomSpeechCoroutine()
    {
        float waitTime = Random.Range(randomSpeechIntervalMin, randomSpeechIntervalMax);
        yield return new WaitForSeconds(waitTime);
        ShowRandomSpeech();
    }

    void ShowRandomSpeech()
    {
        if (Time.time - lastEmotionShownTime < 3f)
        {
            StartRandomSpeechLoop();
            return;
        }

        if (randomSpeechLines.Length > 0)
        {
            string line = randomSpeechLines[Random.Range(0, randomSpeechLines.Length)];
            ShowEmotionBubble(line, null);
        }

        StartRandomSpeechLoop();
    }
    void CheckForFood()
    {
        if (PlayerPrefs.HasKey("food_hunger"))
        {
            int foodHunger = PlayerPrefs.GetInt("food_hunger");
            int foodLove = PlayerPrefs.GetInt("food_love");
            string foodName = PlayerPrefs.GetString("food_name");

            Sprite foodIcon = Resources.Load<Sprite>($"FoodIcons/{foodName}");

            PlayerPrefs.DeleteKey("food_hunger");
            PlayerPrefs.DeleteKey("food_love");
            PlayerPrefs.DeleteKey("food_name");

            StartCoroutine(ThrowFoodAndFeed(foodHunger, foodLove, foodIcon));
        }
    }

    IEnumerator ThrowFoodAndFeed(int hungerInc, int loveInc, Sprite foodIcon)
    {
        if (foodThrower != null)
            foodThrower.ThrowFood(foodIcon);

        yield return new WaitForSeconds(1.2f);
        Feed(hungerInc, loveInc);
    }

    public void Feed(int hungerInc, int loveInc)
    {
        hunger = Mathf.Min(hunger + hungerInc, 100);
        love = Mathf.Min(love + loveInc, 100);

        Debug.Log("Feed called - Set Happy Trigger");
        animator.SetTrigger("Happy");

        currentMood = PetMood.Happy;
        moodStartTime = Time.time;
        ShowEmotionBubble("おいしい！", happyIcon);
        lastEmotionShownTime = Time.time;

        SaveAll();
        UpdateUI();

        
        StartCoroutine(ReturnToIdleAfterDelay(5f));
    }

    IEnumerator ReturnToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentMood = PetMood.Idle;
        animator.SetInteger("Mood", (int)PetMood.Idle);
    }

    void UpdateUI()
    {
        hungerBar.value = hunger;
        loveBar.value = love;
        cleanBar.value = clean;

        coinText.text = petCoins.ToString();
    }

    void SaveAll()
    {
        PlayerPrefs.SetInt("hunger", hunger);
        PlayerPrefs.SetInt("love", love);
        PlayerPrefs.SetInt("clean", clean);
        PlayerPrefs.Save();
    }

    public void GoToShop()
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);

        PlayerPrefs.SetInt("petCoins", petCoins);
        PlayerPrefs.Save();

        SceneManager.LoadScene("FoodShopScene");
    }

    public void GoToMiniGame()
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);

        PlayerPrefs.SetInt("petCoins", petCoins);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MiniGameScene");
    }
}