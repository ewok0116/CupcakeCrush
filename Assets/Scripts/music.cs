using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [SerializeField] private AudioClip mapMusic;     // harita & menü şarkısı
    [SerializeField] private AudioClip levelMusic;   // level şarkısı

    private AudioSource source;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();

        // "her sahne yüklendiğinde bana haber ver" aboneliği
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // sahne adına göre şarkı seç
        AudioClip target = scene.name.StartsWith("Level") ? levelMusic : mapMusic;

        if (source.clip != target)   // zaten o şarkı çalıyorsa baştan başlatma
        {
            source.clip = target;
            source.Play();
        }
    }
}