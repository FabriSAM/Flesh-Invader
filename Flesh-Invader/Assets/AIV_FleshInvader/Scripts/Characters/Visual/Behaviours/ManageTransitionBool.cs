using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTransitionBool : StateMachineBehaviour
{
    [SerializeField]
    private string parameterName;
    [SerializeField]
    private bool newBoolValue;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAnimatorParameter(animator, parameterName, newBoolValue);
    }

    public void SetAnimatorParameter(Animator animator, string name, bool value)
    {
        animator.SetBool(Animator.StringToHash(name), value);
    }
}
