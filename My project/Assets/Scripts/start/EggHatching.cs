using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EggHatching : MonoBehaviour
{
    public Text countdownText;
    public Text statusText;
    public GameObject eggImage;
    public GameObject hatchEffect;

    public AudioSource audioSource;
    public AudioClip countdownClip321;

    void Start()
    {
        StartCoroutine(HatchSequence());
    }

    void Update()
    {
        if (eggImage != null)
        {
            eggImage.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 5f) * 5f);
        }
    }

    IEnumerator HatchSequence()
    {
        statusText.text = "ペットが生まれます...";
        eggImage.SetActive(true);

        yield return new WaitForSeconds(1f);

        // 🔊 เล่นเสียง 3-2-1 พร้อมกับแสดงตัวเลข
        if (audioSource != null && countdownClip321 != null)
        {
            audioSource.PlayOneShot(countdownClip321);
        }

        // ⏱ แสดงตัวเลขตรงกับเสียง
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "🎉";

        if (hatchEffect != null)
            hatchEffect.SetActive(true);

        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetInt("petCoins", 30);
        PlayerPrefs.Save();

        SceneManager.LoadScene("PetScene");
    }
}