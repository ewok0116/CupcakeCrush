using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RandomCupcakeGen : MonoBehaviour
{
    [SerializeField] private Sprite cupcakeyellow;
    [SerializeField] private Sprite cupcakepink;
    [SerializeField] private Sprite cupcakegreen;
    [SerializeField] private Sprite cupcakeblue;
    [SerializeField] private GameObject cupcakePrefab;
    [SerializeField] private Transform panel;
    [SerializeField] private int columns = 4;
    [SerializeField] private int rows = 8;

    [SerializeField] public int targetScore = 500;
    [SerializeField] private int maxSwaps = 20;
    [SerializeField] private Cam1 hud;
    public int LastPlayedLevel;

    private GameObject[,] grid;
    private float cellW, cellH;
    private RectTransform panelRect;
    private Sprite[] sprites;

    private int score = 0;
    private int swapsLeft;
    private bool scoringEnabled = false;
    private bool levelEnded = false;
    public bool isWin;
    [SerializeField] private AudioClip popSound;
    AudioSource audioSource;

    void Start()
    {

        // Start'a:
        audioSource = gameObject.AddComponent<AudioSource>();

        sprites = new Sprite[] { cupcakeyellow, cupcakepink, cupcakegreen, cupcakeblue };

        panelRect = panel.GetComponent<RectTransform>();
        cellW = panelRect.rect.width / columns;
        cellH = panelRect.rect.height / rows;

        grid = new GameObject[columns, rows];
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                CreateCupcake(x, y);

        ResolveMatches();        // açılış temizliği — puansız
        scoringEnabled = true;
        UpdateScoreText();

        swapsLeft = maxSwaps;
        if (hud != null) hud.SetMoves(swapsLeft);
    }

    // ---------- ÜRETİM ----------

    void CreateCupcake(int x, int y)
    {
        GameObject c = Instantiate(cupcakePrefab, panel);
        RectTransform rt = c.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);

        float size = Mathf.Min(cellW, cellH) * 0.9f;
        rt.sizeDelta = new Vector2(size, size);
        rt.anchoredPosition = CellToPos(x, y);

        c.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];

        Cupcake cp = c.GetComponent<Cupcake>();
        cp.x = x; cp.y = y; cp.board = this;

        grid[x, y] = c;
    }

    Vector2 CellToPos(int x, int y)
    {
        return new Vector2(
            (x + 0.5f) * cellW - panelRect.rect.width / 2f,
            (y + 0.5f) * cellH - panelRect.rect.height / 2f);
    }

    Sprite SpriteAt(int x, int y)
    {
        if (grid[x, y] == null) return null;
        return grid[x, y].GetComponent<Image>().sprite;
    }

    // ---------- SWAP ----------

    public void TrySwap(int x, int y, int dx, int dy)
    {
        if (levelEnded) return;

        int nx = x + dx, ny = y + dy;
        if (nx < 0 || nx >= columns || ny < 0 || ny >= rows) return;

        Swap(x, y, nx, ny);

        if (FindMatches().Count > 0)
        {
            swapsLeft--;                              // sadece başarılı hamle sayılır
            if (hud != null) hud.SetMoves(swapsLeft);

            ResolveMatches();

            if (!levelEnded && swapsLeft <= 0)
                LoseLevel();
        }
        else
        {
            Swap(x, y, nx, ny);   // geri al, hamle yanmaz
        }
    }

    void Swap(int x1, int y1, int x2, int y2)
    {
        GameObject a = grid[x1, y1];
        GameObject b = grid[x2, y2];

        grid[x1, y1] = b;
        grid[x2, y2] = a;

        a.GetComponent<RectTransform>().anchoredPosition = CellToPos(x2, y2);
        b.GetComponent<RectTransform>().anchoredPosition = CellToPos(x1, y1);

        Cupcake ca = a.GetComponent<Cupcake>(); ca.x = x2; ca.y = y2;
        Cupcake cb = b.GetComponent<Cupcake>(); cb.x = x1; cb.y = y1;
    }

    // ---------- EŞLEŞME ----------

    HashSet<GameObject> FindMatches()
    {
        HashSet<GameObject> matched = new HashSet<GameObject>();

        for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns - 2; x++)
            {
                Sprite s = SpriteAt(x, y);
                if (s != null && s == SpriteAt(x + 1, y) && s == SpriteAt(x + 2, y))
                {
                    matched.Add(grid[x, y]);
                    matched.Add(grid[x + 1, y]);
                    matched.Add(grid[x + 2, y]);
                }
            }

        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows - 2; y++)
            {
                Sprite s = SpriteAt(x, y);
                if (s != null && s == SpriteAt(x, y + 1) && s == SpriteAt(x, y + 2))
                {
                    matched.Add(grid[x, y]);
                    matched.Add(grid[x, y + 1]);
                    matched.Add(grid[x, y + 2]);
                }
            }

        return matched;
    }

    // ---------- PATLAT + ÇÖKERT + DOLDUR ----------

    void ResolveMatches()
    {
        var matched = FindMatches();

        while (matched.Count > 0)
        {
            if (scoringEnabled)
                AddScore(matched.Count * 10);   // 3'lü→30, 4'lü→40...
                                                //olveMatches'te, Destroy döngüsünden önce:
            if (scoringEnabled && popSound != null)
                audioSource.PlayOneShot(popSound);
            foreach (GameObject go in matched)
            {
                Cupcake cp = go.GetComponent<Cupcake>();
                grid[cp.x, cp.y] = null;
                Destroy(go);
            }

            Collapse();
            Refill();

            matched = FindMatches();
        }
    }

    void Collapse()
    {
        for (int x = 0; x < columns; x++)
        {
            int writeY = 0;

            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y] != null)
                {
                    if (y != writeY)
                    {
                        grid[x, writeY] = grid[x, y];
                        grid[x, y] = null;

                        Cupcake cp = grid[x, writeY].GetComponent<Cupcake>();
                        cp.y = writeY;
                        grid[x, writeY].GetComponent<RectTransform>()
                            .anchoredPosition = CellToPos(x, writeY);
                    }
                    writeY++;
                }
            }
        }
    }

    void Refill()
    {
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                if (grid[x, y] == null)
                    CreateCupcake(x, y);
    }

    // ---------- SKOR / WIN / LOSE ----------

    void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        if (score >= targetScore)
            WinLevel();
    }

    void UpdateScoreText()
    {
        if (hud != null)
            hud.SetScore(score, targetScore);
    }

    void WinLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        string ad = SceneManager.GetActiveScene().name;
        int levelNo = int.Parse(ad.Replace("Level", ""));

        int reached = PlayerPrefs.GetInt("ReachedLevel", 1);
        if (levelNo + 1 > reached)
        {
            PlayerPrefs.SetInt("ReachedLevel", levelNo + 1);
        }

        PlayerPrefs.SetInt("LastPlayedLevel", levelNo);
        PlayerPrefs.Save();
        LastPlayedLevel = levelNo;
        isWin = true;

        SceneManager.LoadScene("WinScene");
    }

    void LoseLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        string ad = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("LastPlayedLevel", int.Parse(ad.Replace("Level", "")));
        PlayerPrefs.Save();
        isWin = false;
        SceneManager.LoadScene("LoseScene");
    }
}