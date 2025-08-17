using UnityEngine;
using UnityEngine.SceneManagement;

public class BathScene : MonoBehaviour
{
    public float duration = 8f; 
    private float timer;

    public Animator petAnimator; 
    private enum PetMood { Idle = 0, Happy = 1, Sleep = 2, Angry = 3 }

    void Start()
    {
        
        if (petAnimator != null)
            petAnimator.SetInteger("Mood", (int)PetMood.Angry);
    }

    void Update()
    {
        timer += Time.deltaTime;

        
        if (timer >= duration - 2f && petAnimator != null) // 2 วินาทีก่อนจบ
            petAnimator.SetInteger("Mood", (int)PetMood.Happy);

        if (timer >= duration)
        {
            PlayerPrefs.SetInt("earned_clean", 100);
            PlayerPrefs.SetInt("earned_love", 20);

         
            PlayerPrefs.SetInt("next_mood", (int)PetMood.Idle);

            SceneManager.LoadScene("PetScene");
        }
    }
}