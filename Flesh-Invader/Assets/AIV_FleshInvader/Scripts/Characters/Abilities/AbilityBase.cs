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
    public abstract void StopAbility();
    public abstract void RegisterInput();
    public abstract void UnRegisterInput();
    #endregion //AbstractMembers

    #region VirtualMembers
    public virtual void Init(Controller characterController)
    {
        this.characterController = characterController;
    }
    #endregion
}
