using UnityEngine;

public class FoodButton : MonoBehaviour
{
    public string foodName; // ตั้งว่า "rice", "milk", "rat"
    public int hunger;
    public int love;
    public int price;
    public Sprite icon;
    public FoodShop shop;

    public AudioSource audioSource;
    public AudioClip clickSound;

    public void OnClick()
    {

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }  

        FoodData food = new FoodData()
        {
            name = foodName.ToLower(), 
            hunger = hunger,
            love = love,
            price = price,
             icon = icon
        };

        shop.SelectFood(food);
    }
}