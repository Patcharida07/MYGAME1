using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameSelect : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    public void OnGame1Selected()
    {
        StartCoroutine(PlaySoundAndLoadScene("FallingGame"));
    }

    public void OnGame2Selected()
    {
        StartCoroutine(PlaySoundAndLoadScene("ShootingMiniGame"));
    }

    public void OnBackButton()
    {
        StartCoroutine(PlaySoundAndLoadScene("PetScene"));
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
            yield return new WaitForSeconds(buttonClickSound.length);
        }

        SceneManager.LoadScene(sceneName);
    }
}
