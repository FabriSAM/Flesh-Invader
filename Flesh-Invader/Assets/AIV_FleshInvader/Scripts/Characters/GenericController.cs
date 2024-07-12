using NotserializableEventManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GenericController : MonoBehaviour
{
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
    void FixedUpdate()
    {
        Move?.Invoke();
    }
    #endregion

    #region InputCallBack
    private void AttackPerformed(InputAction.CallbackContext context)
    {
        PlayerState.Get().InformationController.BulletFired();
        Attack?.Invoke();
    }
    private void PossessionPerformed(InputAction.CallbackContext context)
    {
        if (canUsePossession && !PlayerState.Get().HealthController.DeadStatus)
        {
            PlayerState.Get().InformationController.PossessionShoot();
            possesCoroutine = StartCoroutine(PossesCoroutine());
            Posses?.Invoke();
        }
    }
    public void InteractionPerformed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }

    private void EnablePauseMenu(InputAction.CallbackContext context) {
        if (Time.timeScale == 0) return;
        PlayerState.Get().InformationController.OpenPauseMenu();
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

    #region PublicMethods
    public void InitMe(float possessionCD)
    {
        this.possessionCD = possessionCD;
        canUsePossession = true;
        InputManager.Player.Interact.performed += InteractionPerformed;
        InputManager.Player.Possession.performed += PossessionPerformed;
        InputManager.Player.Attack.performed += AttackPerformed;
        InputManager.Player.PauseEnable.performed += EnablePauseMenu;
    }

    private void OnDisable()
    {
        InputManager.Player.Interact.performed -= InteractionPerformed;
        InputManager.Player.Possession.performed -= PossessionPerformed;
        InputManager.Player.Attack.performed -= AttackPerformed;
        InputManager.Player.PauseEnable.performed -= EnablePauseMenu;
    }
    #endregion
}
