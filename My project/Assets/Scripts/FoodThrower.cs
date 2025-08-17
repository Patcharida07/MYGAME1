using System.Collections; 
using UnityEngine;

public class FoodThrower : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform throwStart;
    public Transform petTarget;

    public AudioSource audioSource;
    public AudioClip throwSound;

    public void ThrowFood(Sprite foodIcon)
    {
        Debug.Log("Throwing food...");
        GameObject food = Instantiate(foodPrefab, throwStart.position, Quaternion.identity);

        var behavior = food.GetComponent<FoodItemBehavior>();
        if (behavior != null)
        {
            behavior.SetFood(foodIcon);
        }

        if (audioSource != null && throwSound != null)
        {
            audioSource.PlayOneShot(throwSound);
        }


        StartCoroutine(MoveAndFade(food));
        Debug.Log($"Throwing food with sprite: {foodIcon?.name}");
    }

    IEnumerator MoveAndFade(GameObject food)
    {
        float duration = 1f;
        float t = 0f;
        Vector3 start = food.transform.position;
        Vector3 end = petTarget.position;

        SpriteRenderer sr = food.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            food.transform.position = Vector3.Lerp(start, end, progress);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - progress);

            yield return null;
        }

        Destroy(food);
    }
}
