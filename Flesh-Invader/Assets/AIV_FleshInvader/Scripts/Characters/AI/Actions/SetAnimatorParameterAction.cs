using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorParameterAction : StateAction
{
    private Animator animator;
    private AnimatorParameterStats parameterValue;
    private bool everyFrame;
    private float parameterTimer;
    private float parameterCounter;

    public SetAnimatorParameterAction(Animator animator, AnimatorParameterStats parameterValue, bool everyFrame = false, float parameterTimer=0)
    {
        this.animator = animator;
        this.parameterValue = parameterValue;
        this.everyFrame = everyFrame;
        this.parameterTimer = parameterTimer;
    }

    public override void OnEnter()
    {
        InternalSetTrigger();
    }

    public override void OnUpdate()
    {
        if (!everyFrame) return;
        
        parameterCounter += Time.deltaTime;
        if(parameterCounter >= parameterTimer)
        {
            InternalSetTrigger();
            parameterCounter = 0;
        }
    }

    private void InternalSetTrigger()
    {
        switch (parameterValue.Type)
        {
            case AnimatorParameterType.INTEGER:
                animator.SetInteger(Animator.StringToHash(parameterValue.parameterName), parameterValue.IntegerValue);
                break;
            case AnimatorParameterType.FLOAT:
                animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), parameterValue.FloatValue);
                break;
            case AnimatorParameterType.BOOL:
                animator.SetBool(Animator.StringToHash(parameterValue.parameterName), parameterValue.BoolValue);
                break;
            case AnimatorParameterType.TRIGGER:
                animator.SetTrigger(Animator.StringToHash(parameterValue.parameterName));
                break;
        }
        
    }
}
