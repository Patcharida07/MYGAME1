using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip clickSound;    
    public void OnStartGame()
    {
        audioSource.PlayOneShot(clickSound);

        
        StartCoroutine(LoadSceneAfterSound());
    }

    private IEnumerator LoadSceneAfterSound()
    {
        yield return new WaitForSeconds(clickSound.length); 
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("petCoins", 30);
        PlayerPrefs.Save();
        SceneManager.LoadScene("HatchingScene");
    }
}
