using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnd : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ActorsInScene;
    List<GameObject> ActorDeath = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        foreach (var actor in ActorsInScene)
        {
            if(actor.activeSelf) continue;
            if (ActorDeath.Contains(actor)) continue;  
            ActorDeath.Add(actor);
        }
        if (ActorsInScene.Count - ActorDeath.Count == 1)
        {            
            SceneManager.LoadScene(0);
        }
    }
}
