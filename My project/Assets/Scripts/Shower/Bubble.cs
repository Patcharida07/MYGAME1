using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float floatSpeed = 1f;

    void Start()
    {
        Destroy(gameObject, 2.5f); // หายไปอัตโนมัติ
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }
}