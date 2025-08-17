using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestBubbleSpawner : MonoBehaviour
{
    public Canvas uiCanvas;
    public Transform bubbleSpawnPoint; // จุดที่ต้องการให้บับเบิลโผล่

    public GameObject emotionBubblePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowEmotionBubble("Hello Bubble!", null);
        }
    }

    void ShowEmotionBubble(string message, Sprite icon)
    {
        if (emotionBubblePrefab == null || uiCanvas == null || bubbleSpawnPoint == null)
        {
            Debug.LogError("Prefab, Canvas หรือ SpawnPoint ไม่ได้กำหนด!");
            return;
        }

        GameObject bubble = Instantiate(emotionBubblePrefab, uiCanvas.transform);

        // แปลงตำแหน่งโลกไปยังตำแหน่งในหน้าจอ UI
        Vector2 screenPos = Camera.main.WorldToScreenPoint(bubbleSpawnPoint.position);
        bubble.transform.position = screenPos;
        bubble.transform.localScale = Vector3.one;

        // หา TMP_Text และ Image ใน Prefab
        TMP_Text textComp = bubble.GetComponentInChildren<TMP_Text>();
        Image iconImage = bubble.transform.Find("EmojiIcon")?.GetComponent<Image>();

        if (textComp != null)
            textComp.text = message;
        if (iconImage != null && icon != null)
            iconImage.sprite = icon;

        Destroy(bubble, 2.5f);
    }
}