using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour
{
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Sprite topbarsprite;
    [SerializeField] private Sprite homeButtonSprite;

    private float minY;
    private Vector3 lastMousePos;
    private Camera cam;
    private GameObject homeButton;   // tıklama kontrolü için sınıf seviyesinde

    void Start()
    {
        cam = GetComponent<Camera>();
        minY = transform.position.y;

        SetupBackground();
    }

    void SetupBackground()
    {
        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        // ---------- BACKGROUND ----------
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(transform);
        bg.transform.localPosition = new Vector3(0, 0, 10);

        SpriteRenderer sr = bg.AddComponent<SpriteRenderer>();
        sr.sprite = backgroundSprite;
        sr.sortingOrder = -10;

        Vector2 bgSize = backgroundSprite.bounds.size;
        bg.transform.localScale = new Vector3(worldW / bgSize.x, worldH / bgSize.y, 1f);

        // ---------- TOP BAR ----------
        GameObject tb = new GameObject("TopBar");
        tb.transform.SetParent(transform);

        SpriteRenderer sr2 = tb.AddComponent<SpriteRenderer>();
        sr2.sprite = topbarsprite;
        sr2.sortingLayerName = "New Layer 3";

        Vector2 barSize = topbarsprite.bounds.size;
        float scale = worldW / barSize.x;
        tb.transform.localScale = new Vector3(scale, scale, 1f);

        float barWorldH = barSize.y * scale;
        tb.transform.localPosition = new Vector3(0, worldH / 2f - barWorldH / 2f, 5);

        // ---------- HOME BUTTON ----------
        homeButton = new GameObject("HomeButton");
        homeButton.transform.SetParent(transform);

        SpriteRenderer sr3 = homeButton.AddComponent<SpriteRenderer>();
        sr3.sprite = homeButtonSprite;
        sr3.sortingLayerName = "New Layer 4";

        Vector2 btnSize = homeButtonSprite.bounds.size;
        float btnScale = (worldH * 0.1f) / btnSize.y;   // yükseklik = ekranın %10'u
        homeButton.transform.localScale = new Vector3(btnScale, btnScale, 1f);

        float btnWorldW = btnSize.x * btnScale;
        homeButton.transform.localPosition = new Vector3(
            -worldW / 2f + btnWorldW / 2f + 0.2f,        // sol kenardan içeride
            worldH / 2f - barWorldH / 2f,                // bar ile aynı hizada
            4f);

        homeButton.AddComponent<CircleCollider2D>();     // tıklama hedef alanı

        // ---------- LEVEL LABEL ----------
        GameObject levelLabel = new GameObject("LevelLabel");
        levelLabel.transform.SetParent(transform);
        levelLabel.transform.localPosition =
            new Vector3(0, worldH / 2f - barWorldH / 2f, 4f);   // barın ortası
        levelLabel.transform.localScale = Vector3.one * 0.5f;

        TextMesh tm = levelLabel.AddComponent<TextMesh>();
        tm.text = "Level " + PlayerPrefs.GetInt("ReachedLevel", 1);
        tm.fontSize = 60;
        tm.characterSize = 0.1f;
        tm.anchor = TextAnchor.MiddleCenter;                    // pivot = yazının ortası
        tm.color = new Color(0.65f, 0.14f, 0.31f);              // koyu pembe (beyaz barda okunsun)

        // TextMesh'in sorting'i kendi üzerinde değil, MeshRenderer'ında ayarlanır:
        levelLabel.GetComponent<MeshRenderer>().sortingLayerName = "New Layer 4";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;

            // home butonuna mı tıklandı?
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);
            if (hit != null && hit.gameObject == homeButton)
            {
                SceneManager.LoadScene("SampleScene");   // ana menü sahnenin gerçek adı
                return;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            float worldPerPixel = (cam.orthographicSize * 2f) / Screen.height;
            float deltaY = (Input.mousePosition.y - lastMousePos.y) * worldPerPixel;

            Vector3 p = transform.position;
            p.y -= deltaY;
            p.y = Mathf.Max(p.y, minY);
            transform.position = p;

            lastMousePos = Input.mousePosition;
        }
    }
}