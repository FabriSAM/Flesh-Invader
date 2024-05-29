using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    [SerializeField]
    private string sceneToLoad;

    private Coroutine changeSceneCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (changeSceneCoroutine != null) return;
        if (Input.GetKey(KeyCode.Space)) {
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine());
        }
    }

    private IEnumerator ChangeSceneCoroutine () {
        var loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!loadScene.isDone) {
            yield return new WaitForEndOfFrame();
        }
    }
}
