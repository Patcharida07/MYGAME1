using UnityEngine;

public class FoodItemBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void SetFood(Sprite icon)
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = icon;
    }
}
