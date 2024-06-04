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


public class GetSpeedAction : StateAction
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private AnimatorParameterStats parameterValue;
    private VectorAxis axisToUse;
    private bool everyFrame;

    public GetSpeedAction(Animator animator, NavMeshAgent navMeshAgent, VectorAxis axis, AnimatorParameterStats parameterValue,  bool everyFrame = false)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;
        this.axisToUse = axis;
        this.parameterValue = parameterValue;
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
                animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), XDirection);                
                break;
            case VectorAxis.Y:
                float YDirection = Vector3.Dot(navMeshAgent.velocity.normalized, navMeshAgent.transform.up);
                animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), YDirection);
                break;
            case VectorAxis.Z:
                float ZDirection = Vector3.Dot(navMeshAgent.velocity.normalized, navMeshAgent.transform.forward);
                animator.SetFloat(Animator.StringToHash(parameterValue.parameterName), ZDirection);
                break;
        }
        
    }
}
