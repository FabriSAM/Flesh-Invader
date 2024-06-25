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
    PlayerStateInformation informationController;
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
    public PlayerStateInformation InformationController { get { return informationController; } }
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
        instance = this;
        levelController.InitMe();        
        genericController.InitMe(possessionCD);
    }

    private void Start()
    {
        if (instance != this) return;
        healthController.InitMe(this);
        missionController.InitMe();
    }
    #endregion
}
