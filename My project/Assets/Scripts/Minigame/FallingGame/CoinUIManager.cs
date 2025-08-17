using UnityEngine;
using TMPro;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;

    void Start()
    {
        CoinManager.ResetCoins(); // Reset ตอนเริ่มเกม
        CoinManager.SetUIManager(this);
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinText.text = "Coins: " + CoinManager.GetCoins();
    }
}
