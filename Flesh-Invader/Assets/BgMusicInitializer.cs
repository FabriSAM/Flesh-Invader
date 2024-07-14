using UnityEngine;

public class BgMusicInitializer : MonoBehaviour
{
    [SerializeField]
    private BackgroundMusic backgroundMusic;

    private void Start()
    {
        AudioManager.Get().InitializeBackgroundMusic();
        AudioManager.Get().ChangeBackgroundMusic(backgroundMusic);
        AudioManager.Get().StartBackgroundMusic();
    }
}
