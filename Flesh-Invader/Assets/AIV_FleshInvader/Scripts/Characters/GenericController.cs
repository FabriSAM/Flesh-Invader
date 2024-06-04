using Codice.CM.Common;
using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GenericController : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private float defaultPossessionCD;
    [SerializeField]
    private PlayerState playerState;
    #endregion

    #region PrivateMembers
    private float possessionCD;
    private bool canUsePossession;
    private Coroutine possesCoroutine;
    #endregion

    #region Action
    public Action Move;
    public Action Attack;
    public Action Interact;
    public Action Rotate;
    public Action Posses;

    public Action Pos2;
    #endregion

    #region Mono
    void Awake()
    {
        possessionCD = defaultPossessionCD;
        canUsePossession = true;
        InputManager.Player.Interact.performed += InteractionPerformed;
        InputManager.Player.Possession.performed += PossessionPerformed;
        InputManager.Player.Attack.performed += AttackPerformed;
        playerState.onLevelChange += OnLevelChange;

        InputManager.Vertical.Pos1.performed += Pos1Performed;
        InputManager.Vertical.Pos2.performed += Pos2Performed;
        InputManager.Vertical.Pos3.performed += Pos3Performed;
    }

    private void Pos3Performed(InputAction.CallbackContext context)
    {
        Pos2Invoke();
    }

    private void Pos2Performed(InputAction.CallbackContext context)
    {
        Pos2Invoke();
        //async load with loading widget
        SceneManager.LoadSceneAsync(2);
    }
    private void Pos1Performed(InputAction.CallbackContext context)
    {
        Pos2Invoke();
        SceneManager.LoadSceneAsync(1);
    }

    private void Pos2Invoke()
    {
        Pos2?.Invoke();
        StopCoroutine(PossesCoroutine());
        possesCoroutine = null;
    }

    void FixedUpdate()
    {
        Move?.Invoke();
    }
    #endregion

    #region InputCallBack
    private void AttackPerformed(InputAction.CallbackContext context)
    {
        Attack?.Invoke();
    }
    private void PossessionPerformed(InputAction.CallbackContext context)
    {
        if (canUsePossession)
        {
            possesCoroutine = StartCoroutine(PossesCoroutine());
            Posses?.Invoke();
        }
    }
    public void InteractionPerformed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }
    #endregion

    #region CallBack
    private void OnLevelChange(int newLevel)
    {
        possessionCD = defaultPossessionCD / newLevel;
    }
    #endregion

    #region Coroutine
    private IEnumerator PossesCoroutine()
    {
        canUsePossession = false;
        CastGlobalEvent(canUsePossession);
        yield return new WaitForSeconds(possessionCD);
        canUsePossession = true;
        CastGlobalEvent(canUsePossession);
    }
    #endregion

    #region CastEvent
    private void CastGlobalEvent(bool state)
    {
        GlobalEventSystem.CastEvent(EventName.PossessionAbilityState,
                EventArgsFactory.PossessionAbilityStateFactory(state));
    }
    #endregion
}
