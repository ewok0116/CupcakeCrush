using TMPro;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject btn;        // prefab
    [SerializeField] private Transform container;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 startPos = new Vector2(0, -3);
    [SerializeField] private float spacingY = 2.5f;

    private int lastButtonNumber = 0;
    private float lastSpawnY;

    void Start()
    {
        for (int i = 0; i < 8; i++)   // başlangıçta ekranı dolduracak kadar üret
            CreateLevel();
    }

    void Update()
    {
        float limit = lastSpawnY - spacingY * 2f;          // alarm çizgisi
        float camTop = cam.transform.position.y + cam.orthographicSize;

        if (camTop > limit)
        {
            CreateLevel();
            CreateLevel();
        }
    }

    void CreateLevel()
    {
        lastButtonNumber++;

        float x = startPos.x + (lastButtonNumber % 2 == 0 ? 1.5f : -1.5f);
        float y = startPos.y + (lastButtonNumber - 1) * spacingY;
        lastSpawnY = y;
        GameObject newBtn = Instantiate(btn, container);
        newBtn.transform.position = new Vector3(x, y, container.position.z);
        newBtn.GetComponentInChildren<TMP_Text>(true).text = lastButtonNumber.ToString();
    }
}