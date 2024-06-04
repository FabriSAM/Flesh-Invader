using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Visual : MonoBehaviour
{

    [SerializeField]
    private SortingGroup sortingGroup;
    [SerializeField]
    private Animator characterAnimator;

    public Animator CharacterAnimator { get { return characterAnimator; } }


    #region Mono
    private void OnEnable()
    {
        InitializeAnimator();
    }
    #endregion

    #region Animator Wrapper Methods
    public void SetAnimatorParameter(string name)
    {
        if (!InternalValidateAnimator(characterAnimator)) return;
        characterAnimator.SetTrigger(Animator.StringToHash(name));
    }

    public void SetAnimatorParameter(string name, bool value)
    {
        if (!InternalValidateAnimator(characterAnimator)) return;
        characterAnimator.SetBool(Animator.StringToHash(name), value);
    }

    public void SetAnimatorParameter(string name, float value)
    {
        if (!InternalValidateAnimator(characterAnimator)) return;
        characterAnimator.SetFloat(Animator.StringToHash(name), value);
    }

    public void SetAnimatorParameter(string name, int value)
    {
        if (!InternalValidateAnimator(characterAnimator)) return;
        characterAnimator.SetInteger(Animator.StringToHash(name), value);
    }

    public void SetAnimatorSpeed(float speed)
    {
        if (!InternalValidateAnimator(characterAnimator)) return;
        characterAnimator.speed = speed;
    }
    #endregion

    #region Visual Methods
    public void ChangeSortingLayer(int sortingLayerID)
    {
        if (!InternalValidateSortingGroup(sortingGroup)) return;
        sortingGroup.sortingLayerID = sortingLayerID;
    }

    public void ChangeOrderInLayer(int orderInLayer)
    {
        if (!InternalValidateSortingGroup(sortingGroup)) return;
        sortingGroup.sortingOrder = orderInLayer;
    }
    #endregion

    #region Internal Methods
    private bool InternalValidateAnimator(Animator animator)
    {
        return animator;
    }
    private bool InternalValidateSortingGroup(SortingGroup sortingGroup)
    {
        return sortingGroup;
    }
    private void InitializeAnimator()
    {
        IAnimatorInitializer[] initers = GetComponents<IAnimatorInitializer>();

        foreach (IAnimatorInitializer initializer in initers)
        {
            if (initializer == null) return;
            initializer.InitializeAnimatorParams(this);
        }
    }
    #endregion

}
