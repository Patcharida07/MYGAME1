using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static int coinCount = 0;
    private static CoinUIManager uiManager;

    public static void SetUIManager(CoinUIManager manager)
    {
        uiManager = manager;
    }

    public static void ResetCoins()
    {
        coinCount = 0;
    }

    public static void AddCoin()
    {
        coinCount++;
        if (uiManager != null)
            uiManager.UpdateCoinUI();
    }

    public static int GetCoins()
    {
        return coinCount;
    }
}
