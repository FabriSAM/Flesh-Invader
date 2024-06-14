using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTransitionInteger : StateMachineBehaviour
{
    [SerializeField]
    private string parameterName;
    [SerializeField]
    private int newIntegerValue;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAnimatorParameter(animator, parameterName, newIntegerValue);
    }

    public void SetAnimatorParameter(Animator animator, string name, int value)
    {
        animator.SetInteger(Animator.StringToHash(name), value);
    }
}
