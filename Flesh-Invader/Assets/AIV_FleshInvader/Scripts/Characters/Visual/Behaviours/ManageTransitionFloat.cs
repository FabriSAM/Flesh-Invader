using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTransitionFloat : StateMachineBehaviour
{
    [SerializeField]
    private string parameterName;
    [SerializeField]
    private float newFloatValue;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAnimatorParameter(animator, parameterName, newFloatValue);
    }

    public void SetAnimatorParameter(Animator animator, string name, float value)
    {
        animator.SetFloat(Animator.StringToHash(name), value);
    }
}
