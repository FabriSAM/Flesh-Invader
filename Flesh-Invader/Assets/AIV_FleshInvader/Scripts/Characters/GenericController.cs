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
    void Awake()
    {
        possessionCD = defaultPossessionCD;
        canUsePossession = true;
        InputManager.Player.Interact.performed += InteractionPerformed;
        InputManager.Player.Possession.performed += PossessionPerformed;
        InputManager.Player.Attack.performed += AttackPerformed;
        InputManager.Player.PauseEnable.performed += EnablePauseMenu;
        playerState.onLevelChange += OnLevelChange;

        InputManager.Vertical.Pos1.performed += Pos1Performed;
    }

    

    private void OnDestroy()
    {
        InputManager.Player.Interact.performed -= InteractionPerformed;
        InputManager.Player.Possession.performed -= PossessionPerformed;
        InputManager.Player.Attack.performed -= AttackPerformed;
        InputManager.Vertical.Pos1.performed -= Pos1Performed;
    }

    private void Pos1Performed(InputAction.CallbackContext context)
    {
        GameObject.Find("Pooler").GetComponentInChildren(typeof(CharacterSpawner), true).gameObject.SetActive(true);
        Pos2Invoke();
        //async load with loading widget
        SceneManager.LoadSceneAsync(2);
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
        if (canUsePossession && !PlayerState.Get().HealthController.DeadStatus)
        {
            possesCoroutine = StartCoroutine(PossesCoroutine());
            Posses?.Invoke();
        }
    }
    public void InteractionPerformed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }

    private void EnablePauseMenu(InputAction.CallbackContext context) {
        PauseMenuButtonHandler.Instance.OnPauseMenuTriggerEvent?.Invoke();
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
    }
    #endregion
}
