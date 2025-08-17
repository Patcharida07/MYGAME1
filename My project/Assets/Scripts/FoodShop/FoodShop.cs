using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class FoodData
{
    public string name;
    public int hunger, love, price;
    public Sprite icon;
}

public class FoodShop : MonoBehaviour
{
    public FoodData selectedFood;
    public FoodUIManager ui;
    public int playerMoney;

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        playerMoney = PlayerPrefs.GetInt("petCoins", 0); // ✅ ดึงจาก petCoins
        ui.UpdateMoney(playerMoney);
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        PlayerPrefs.SetInt("player_money", playerMoney);
        PlayerPrefs.Save();
        ui.UpdateMoney(playerMoney);
        Debug.Log("เพิ่มเงิน: " + amount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))  // กด M เพื่อเพิ่มเงิน 50
        {
            AddMoney(100);
        }
    }
    public void SelectFood(FoodData food)
    {
        selectedFood = food;
        ui.UpdateFoodUI(food, playerMoney);
    }

    public void BuyFood()
    {
        if (selectedFood == null) return;

        int price = selectedFood.price;

        if (playerMoney >= price)
        {
            playerMoney -= price;

         
            PlayerPrefs.SetInt("food_hunger", selectedFood.hunger);
            PlayerPrefs.SetInt("food_love", selectedFood.love);
            PlayerPrefs.SetString("food_name", selectedFood.name);

            PlayerPrefs.SetInt("petCoins", playerMoney);
            PlayerPrefs.Save();

            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            SceneManager.LoadScene("PetScene");
        }
        else
        {
            Debug.Log("เงินไม่พอซื้ออาหาร");
        }
    }

    public void Back()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("PetScene");

    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.PlayOneShot(clickSound);

        yield return new WaitForSeconds(clickSound.length);

        SceneManager.LoadScene("PetScene");
    }
}