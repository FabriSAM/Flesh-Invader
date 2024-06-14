using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTransitionTrigger : StateMachineBehaviour
{
    [SerializeField]
    private string parameterTriggerName;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAnimatorParameter(animator, parameterTriggerName);
    }

    public void SetAnimatorParameter(Animator animator, string name)
    {
        animator.SetTrigger(Animator.StringToHash(name));
    }
}
