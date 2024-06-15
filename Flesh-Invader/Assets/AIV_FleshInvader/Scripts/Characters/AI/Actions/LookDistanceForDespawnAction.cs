using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDistanceForDespawnAction : StateAction
{
    private GameObject actor;
    private float distanceForDespawn;

    private float checkDistanceFrequency;
    private float checkDistanceTimerCounter;

    public LookDistanceForDespawnAction(GameObject actor, float distanceForDespawn, float checkDistanceFrequency)
    {
        this.actor = actor;
        this.distanceForDespawn = distanceForDespawn;

        this.checkDistanceFrequency = checkDistanceFrequency;
    }

    public override void OnUpdate()
    {
        checkDistanceTimerCounter += Time.deltaTime;

        if(checkDistanceTimerCounter >= checkDistanceFrequency)
        {
            InternalCheckDistance();
            checkDistanceTimerCounter = 0;
        }

    }

    private void InternalCheckDistance()
    {
        if((distanceForDespawn * distanceForDespawn) <= Vector3.SqrMagnitude(actor.transform.position - PlayerState.Get().CurrentPlayer.transform.position))
        {
            actor.SetActive(false);
            GlobalEventSystem.CastEvent(EventName.EnemyDeath, EventArgsFactory.EnemyDeathFactory());
        }
    }
}
