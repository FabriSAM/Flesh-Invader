using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region StaticMembers
    private static AudioManager instance;

    public static AudioManager Get() {
        if (instance != null) return instance;
        instance = FindObjectOfType<AudioManager>();
        return instance;
    }
    #endregion

    #region Mono
    /*private void Start() {
        PlayOneShot("Background", "Background");
    }*/
    #endregion

    #region Internal
    public void PlayOneShot(string eventName = null, string bankName = null) {
        if(string.IsNullOrEmpty(eventName)) return;
        string eventPath = "event:";
        if (!string.IsNullOrEmpty(bankName)) {
            eventPath += $"/{bankName}";
        }
        eventPath += $"/{eventName}";
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, Vector3.zero);
    }
    #endregion
}