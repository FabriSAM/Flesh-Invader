using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    GenericController genericController;
    [SerializeField]
    PlayerStateMission missionController;
    [SerializeField]
    PlayerStateLevel levelController;
    [SerializeField]
    PlayerStateHealth healthController;
    [SerializeField]
    float possessionCD;
    #endregion

    #region PrivateMembers
    private GameObject currentPlayer;
    #endregion

    #region Properties
    public GameObject CurrentPlayer
    {
        get
        {
            if (currentPlayer == null)
            {
                return gameObject;
            }
            return currentPlayer;
        }
        set { currentPlayer = value; }
    }
    public GenericController GenericController { get { return genericController; } }
    public PlayerStateMission MissionController { get { return missionController; } }
    public PlayerStateLevel LevelController { get { return levelController; } }
    public PlayerStateHealth HealthController { get { return healthController; } }
    #endregion
    
    #region StaticMembers
    private static PlayerState instance;

    public static PlayerState Get()
    {
        if (instance != null) return instance;
        instance = GameObject.FindObjectOfType<PlayerState>();
        return instance;
    }
    #endregion

    #region MonoCallbacks
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        levelController.InitMe();
        missionController.InitMe();
        genericController.InitMe(possessionCD);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (instance != this) return;
        healthController.InitMe(this);
    }
    #endregion
}
