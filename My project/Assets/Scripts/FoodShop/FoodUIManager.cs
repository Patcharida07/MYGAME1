using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoodUIManager : MonoBehaviour
{
    public TMP_Text foodNameText, hungerText, loveText, moneyText;
    public Image foodImage;
    public Button buyButton;
    public TMP_Text buyButtonText;
    public FoodShop shop;
    public Vector2 foodImageSize = new Vector2(100, 100);

    public void UpdateFoodUI(FoodData food, int playerMoney)
    {
        foodNameText.text = food.name;
        hungerText.text = $"Feed +{food.hunger}";
        loveText.text = $"Love +{food.love}";
        foodImage.sprite = food.icon;

        // ปรับขนาดภาพอาหาร
        foodImage.rectTransform.sizeDelta = foodImageSize;

        moneyText.text = $"Money: {playerMoney}";
        buyButtonText.text = food.price.ToString();
    }

    public void UpdateMoney(int money)
    {
        moneyText.text = $"Money: {money}";
    }

    public void OnBuyButton() => shop.BuyFood();
}