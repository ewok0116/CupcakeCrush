using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoSceneBehaviour : MonoBehaviour
{
    void OnMouseDown()
    {
        int lastLevel = PlayerPrefs.GetInt("LastPlayedLevel", 1);
        string buSahne = SceneManager.GetActiveScene().name;

        if (buSahne == "WinScene")
            SceneManager.LoadScene("Level" + (lastLevel + 1));   // kazandı → sonraki
        else
            SceneManager.LoadScene("Level" + lastLevel);          // kaybetti → tekrar
    }
}