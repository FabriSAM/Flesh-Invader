using Codice.CM.Common;
using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
