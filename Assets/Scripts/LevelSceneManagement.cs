using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;

    private int levelNumber;
    private bool isLocked;

    void Start()
    {
        string[] parcalar = gameObject.name.Split('_');
        levelNumber = int.Parse(parcalar[1]);

        CheckCupcakeType();
    }

    public void CheckCupcakeType()
    {
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);
        isLocked = levelNumber > reachedLevel;

        GetComponent<SpriteRenderer>().sprite = isLocked ? lockedSprite : unlockedSprite;
    }

    void OnMouseDown()
    {
        Debug.Log("Tıklandı: " + gameObject.name);

        if (isLocked) return;
        SceneManager.LoadScene("Level" + levelNumber);
    }
}