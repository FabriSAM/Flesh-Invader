using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    GenericController genericController;
    #endregion

    #region PrivateMembers
    private Transform playerTransform;
    #endregion

    #region Properties
    public Transform PlayerTransform 
    { 
        get 
        { 
            if(playerTransform == null)
            {
                return gameObject.transform; 
            }
            return playerTransform; 
        } 
        set { playerTransform = value; } 
    }
    public GenericController GenericController { get { return genericController; } }
    #endregion

    #region Action
    public Action<int> onLevelChange;
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (instance != this) return;
    }
    #endregion
}
