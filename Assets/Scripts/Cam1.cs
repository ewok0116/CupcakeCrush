using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam1 : MonoBehaviour
{
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Sprite topbarsprite;
    [SerializeField] private Sprite homeButtonSprite;

    private Vector3 lastMousePos;
    private Camera cam;
    private GameObject homeButton;
    public TextMesh scoreLabel;
    public TextMesh movesLabel;

    void Awake()
    {
        cam = GetComponent<Camera>();
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

        Vector3 size = topbarsprite.bounds.size;
        Vector2 barSize = size;
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
        float btnScale = (worldH * 0.1f) / btnSize.y;
        homeButton.transform.localScale = new Vector3(btnScale, btnScale, 1f);

        float btnWorldW = btnSize.x * btnScale;
        homeButton.transform.localPosition = new Vector3(
            -worldW / 2f + btnWorldW / 2f + 0.2f,
            worldH / 2f - barWorldH / 2f,
            4f);

        homeButton.AddComponent<CircleCollider2D>();

        // ---------- LEVEL LABEL (oynanan level) ----------
        GameObject levelLabel = new GameObject("LevelLabel");
        levelLabel.transform.SetParent(transform);
        levelLabel.transform.localPosition = new Vector3(0, worldH / 2f - barWorldH / 2f, 4f);
        levelLabel.transform.localScale = Vector3.one * 0.5f;

        TextMesh tm = levelLabel.AddComponent<TextMesh>();
        string sahneAdi = SceneManager.GetActiveScene().name;
        tm.text = sahneAdi.Replace("Level", "Level ");
        tm.fontSize = 60;
        tm.characterSize = 0.1f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.color = new Color(0.65f, 0.14f, 0.31f);
        levelLabel.GetComponent<MeshRenderer>().sortingLayerName = "New Layer 4";

        // ---------- SCORE LABEL ----------
        GameObject scoreObj = new GameObject("ScoreLabel");
        scoreObj.transform.SetParent(transform);
        scoreObj.transform.localPosition = new Vector3(
            worldW / 2f - 0.3f,
            worldH / 2f - barWorldH / 2f,
            4f);
        scoreObj.transform.localScale = Vector3.one * 0.5f;

        scoreLabel = scoreObj.AddComponent<TextMesh>();
        scoreLabel.text = "0 / ?";
        scoreLabel.fontSize = 60;
        scoreLabel.characterSize = 0.1f;
        scoreLabel.anchor = TextAnchor.MiddleRight;
        scoreLabel.color = new Color(0.65f, 0.14f, 0.31f);
        scoreObj.GetComponent<MeshRenderer>().sortingLayerName = "New Layer 4";

        // ---------- MOVES LABEL ----------
        GameObject movesObj = new GameObject("MovesLabel");
        movesObj.transform.SetParent(transform);
        movesObj.transform.localPosition = new Vector3(
            worldW / 2f - 0.3f,
            worldH / 2f - barWorldH * 1.1f,
            4f);
        movesObj.transform.localScale = Vector3.one * 0.4f;

        movesLabel = movesObj.AddComponent<TextMesh>();
        movesLabel.text = "Hamle: ?";
        movesLabel.fontSize = 60;
        movesLabel.characterSize = 0.1f;
        movesLabel.anchor = TextAnchor.MiddleRight;
        movesLabel.color = new Color(0.65f, 0.14f, 0.31f);
        movesObj.GetComponent<MeshRenderer>().sortingLayerName = "New Layer 4";

    }

    public void SetScore(int score, int target)
    {
        if (scoreLabel != null)
            scoreLabel.text = score + " / " + target;
    }

    public void SetMoves(int moves)
    {
        if (movesLabel != null)
            movesLabel.text = "Hamle: " + moves;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;

            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);
            if (hit != null && hit.gameObject == homeButton)
            {
                SceneManager.LoadScene("SampleScene");
                return;
            }
        }
    }
}