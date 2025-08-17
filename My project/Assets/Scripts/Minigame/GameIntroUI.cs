using UnityEngine;
using System.Collections;

public class GameIntroUI : MonoBehaviour
{
    public GameObject introPanel;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        Time.timeScale = 0f;
        introPanel.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(PlaySoundAndStart());
    }

    private IEnumerator PlaySoundAndStart()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSecondsRealtime(clickSound.length); 
        }

        introPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}