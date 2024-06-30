using UnityEngine;

public enum BackgroundMusic {
    None = 0,
    MainMenu = 1,
    Gameplay = 2,
    EndScreen = 3
}

public class AudioManager : MonoBehaviour {

    #region StaticMembers
    private static AudioManager instance;

    public static AudioManager Get() {
        if (instance != null) return instance;
        instance = FindObjectOfType<AudioManager>();
        return instance;
    }
    #endregion

    #region InternalVariables
    private string backgroundMusicEventPath = "event:/Background/Background";
    private string backgroundMusicAreaParameterName = "Area";
    #endregion

    #region Instances
    private FMOD.Studio.EventInstance backgroundMusic;
    #endregion

    #region Mono
    private void Awake() {
        DontDestroyOnLoad(this);
    }
    #endregion

    #region Internal
    public void InitializeBackgroundMusic() {
        backgroundMusic = FMODUnity.RuntimeManager.CreateInstance(backgroundMusicEventPath);
    }

    public void StartBackgroundMusic() {
        backgroundMusic.start();
    }

    public void StopBackgroundMusic(FMOD.Studio.STOP_MODE stopMode) {
        backgroundMusic.stop(stopMode);
    }

    public void ReleaseBackgroundMusic() {
        backgroundMusic.release();
    }

    public void PlayOneShot(string eventName = null, string bankName = null) {
        if(string.IsNullOrEmpty(eventName)) return;
        string eventPath = "event:";
        if (!string.IsNullOrEmpty(bankName)) {
            eventPath += $"/{bankName}";
        }
        eventPath += $"/{eventName}";
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, Vector3.zero);
    }

    public void ChangeBackgroundMusic(BackgroundMusic area) {
        if (area == BackgroundMusic.None) return;
        backgroundMusic.setParameterByName(backgroundMusicAreaParameterName, (int)area);
    }
    #endregion
}