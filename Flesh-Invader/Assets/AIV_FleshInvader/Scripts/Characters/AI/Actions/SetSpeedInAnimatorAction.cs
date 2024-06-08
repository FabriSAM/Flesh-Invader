using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum VectorAxis
{
    X,
    Y,
    Z
}


public class SetSpeedInAnimatorAction : StateAction
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private AnimatorParameterStats parameterValue;
    private VectorAxis axisToUse;
    private bool everyFrame;
    private float threshold;

    public SetSpeedInAnimatorAction(Animator animator, NavMeshAgent navMeshAgent, VectorAxis axis, AnimatorParameterStats parameterValue, float threshold, bool everyFrame = false)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;
        this.axisToUse = axis;
        this.parameterValue = parameterValue;
        this.threshold = threshold;
        this.everyFrame = everyFrame;

    }

    public override void OnEnter()
    {
        InternalSetTrigger();
    }

    public override void OnUpdate()
    {
        if (!everyFrame) return;
        InternalSetTrigger();
    }

    private void InternalSetTrigger()
    {
        switch (axisToUse)
        {
            case VectorAxis.X:
                float XDirection = Vector3.Dot(navMeshAgent.velocity.normalized, navMeshAgent.transform.right);
                if (Mathf.Abs(XDirection) > threshold)
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), XDirection);
                else
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), 0);
                break;
            case VectorAxis.Y:
                float YDirection = Vector3.Dot(navMeshAgent.velocity.normalized, navMeshAgent.transform.up);
                if (Mathf.Abs(YDirection) > threshold)
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), YDirection);
                else
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), 0);
                break;
            case VectorAxis.Z:
                float ZDirection = Vector3.Dot(navMeshAgent.velocity.normalized, navMeshAgent.transform.forward);
                if (Mathf.Abs(ZDirection) > threshold)
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), ZDirection);
                else
                    animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), 0);
                break;
        }
        
    }
}
