using JetBrains.Annotations;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SahneGec : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventdata)
    {
        SceneManager.LoadScene(1);
    }
}