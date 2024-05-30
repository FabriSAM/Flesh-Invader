using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase:MonoBehaviour
{
    #region References
    protected Controller characterController;
    #endregion //References

    #region ProtectedMembers
    protected bool isPrevented;
    #endregion //ProtectedMembers

    #region AbstractMembers
    public abstract void OnInputDisabled();
    public abstract void OnInputEnabled();
    public abstract void StopAbility();
    #endregion //AbstractMembers

    #region VirtualMembers
    public virtual void Init(Controller characterController)
    {
        this.characterController = characterController;
        characterController.OnPosses += OnInputEnabled;
        characterController.OnUnposses += OnInputDisabled;
    }
    #endregion
}
