using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    public void OnFeedButton()
    {
        StartCoroutine(PlaySoundAndLoadScene("FoodShopScene"));
    }

    public void OnCleanButton()
    {
        StartCoroutine(PlaySoundAndLoadScene("BathScene"));
    }

    public void OnMinigameButton()
    {
        StartCoroutine(PlaySoundAndLoadScene("MinigameSelectScene"));
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);

        yield return new WaitForSeconds(buttonClickSound.length);

        SceneManager.LoadScene(sceneName);
    }
}