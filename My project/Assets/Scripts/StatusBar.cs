using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Slider hungerSlider;
    public Slider loveSlider;
    public Slider cleanSlider;

    void Update()
    {
        hungerSlider.value = PlayerPrefs.GetInt("hunger", 100);
        loveSlider.value = PlayerPrefs.GetInt("love", 100);
        cleanSlider.value = PlayerPrefs.GetInt("clean", 100);
    }
}