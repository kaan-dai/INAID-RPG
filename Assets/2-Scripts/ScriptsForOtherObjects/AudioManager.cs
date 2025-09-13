using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusicSource;
    public AudioClip bossRoomMusic;
    public AudioClip waitingHallMusic;
    public AudioClip CrystalMusic;
    public AudioClip DesertMusic;
    public AudioClip RockyMusic;
    public AudioClip SnowMusic;





    void Awake()
    {
    if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this newly created duplicate.
            return; // Exit to prevent further execution of this Awake method.
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep the original AudioManager across scenes.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "WaitingHallScene"){
            StopMusic();
            }
        switch (scene.name)
        {
            case "WaitingHallScene":
                PlayMusic(waitingHallMusic);
                break;
            case "CrystalScene":
                PlayMusic(CrystalMusic);
                break;
            case "DesertScene":
                PlayMusic(DesertMusic);
                break;
            case "RockyScene":
                PlayMusic(RockyMusic);
                break;
            case "SnowScene":
                PlayMusic(SnowMusic);
                break;   

        }
    }

    public void PlayMusic(AudioClip clip)
    {
    if (backgroundMusicSource.clip != clip)
        {
            backgroundMusicSource.Stop(); // Ensure the current music is stopped.
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.Play();
        }
    }

    public void StopMusic()
    {
        backgroundMusicSource.Stop();
    }
    public void PlayBossRoomMusic()
    {
        PlayMusic(bossRoomMusic);
    }
    public void ChangeMusic()
    {
    StopMusic();
    OnDestroy();
    PlayBossRoomMusic();
    }
}
