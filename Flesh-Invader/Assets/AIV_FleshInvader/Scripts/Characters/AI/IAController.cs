using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class EnvTargetPoint
{
    public Vector3  TargetPosition;
    public float    occupationRad;
    public bool     isActive;
    public bool     IsOccupied;

    public EnvTargetPoint(Vector3 position, float occupationRadius)
    {
        TargetPosition = position;
        occupationRad = occupationRadius;
        isActive = true;
        IsOccupied = false;
    }

    public EnvTargetPoint() : this(Vector3.zero, 1) { }


}
public struct EnvTargetFilters
{
    // It would be nice to manage this filters with the decorator pattern
    public bool ActivationStatus;
    public bool InCollision;
    public bool Occupation;

    public EnvTargetFilters(bool checkActive, bool checkCollisions, bool checkOccupation)
    {
        ActivationStatus = checkActive;
        InCollision = checkCollisions;
        Occupation = checkOccupation;
    }
}


public class IAController : MonoBehaviour
{
    private static EnvTargetPoint[] targetPoints;

    #region SerializedFields
    [Header("IA Targets")]
    [SerializeField]
    private int targetNumber;
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private float targetUpdateTime;
    private float targetUpdateCounter;
    #endregion

    private Vector3[] targetsNormalizedPositions;

    private void Awake()
    {
        targetPoints = new EnvTargetPoint[targetNumber];
        for (int i = 0; i < targetNumber; i++)
        {
            targetPoints[i] = new EnvTargetPoint();
        }
        targetsNormalizedPositions = new Vector3[targetNumber];

        float radDistance = (2 * (float)Math.PI) / targetNumber;
        for (int i = 0; i < targetNumber; i++)
        {
            targetsNormalizedPositions[i] = new Vector3(Mathf.Cos(radDistance * i), 0, Mathf.Sin(radDistance * i));
        }
    }

    private void Update()
    {
        targetUpdateCounter += Time.deltaTime;

        if(targetUpdateCounter > targetUpdateTime)
        {
            targetUpdateCounter = 0;

            for (int i=0; i<targetNumber; i++)
            {
                targetPoints[i].TargetPosition = PlayerState.Get().CurrentPlayer.transform.position + (targetsNormalizedPositions[i] * targetRadius);
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        
        for (int i = 0; i < targetNumber; i++)
        {
            if (targetPoints[i] != null)
            {
                if (targetPoints[i].IsOccupied) Gizmos.color = Color.blue;
                else Gizmos.color = Color.green;

                Gizmos.DrawWireSphere(targetPoints[i].TargetPosition, targetPoints[i].occupationRad);
            }
        }
    }

    public EnvTargetPoint[] TargetPoints{ get { return targetPoints; } }

    #region StaticMethods
    public static void CheckNearestTarget(Vector3 actorPosition, out bool freePosition, out EnvTargetPoint target)
    {
        EnvTargetPoint[] validTargetPoint = GetValidTargets(new EnvTargetFilters(true, false, true));
        if(validTargetPoint.Length > 0)
        {
            float distance = float.PositiveInfinity;
            EnvTargetPoint chosenTargetPoint = validTargetPoint[0];

            foreach (EnvTargetPoint targetpoint in validTargetPoint)
            {
                float targetDistance = Vector3.SqrMagnitude(targetpoint.TargetPosition - actorPosition);
                if(targetDistance < distance)
                {
                    chosenTargetPoint = targetpoint;
                }
            }

            // Results
            freePosition = true;
            target = chosenTargetPoint;

        }

        // Results
        freePosition = false;
        target = new EnvTargetPoint();
    } 

    public static EnvTargetPoint[] GetValidTargets(EnvTargetFilters filters)
    {
        List<EnvTargetPoint> validTargets = targetPoints.ToList();

        if (filters.ActivationStatus)
        {
            foreach (EnvTargetPoint targetPoint in validTargets)
            {
               if (!targetPoint.isActive)
                    validTargets.Remove(targetPoint);
            }
        }


        if (filters.InCollision)
        {
            foreach (EnvTargetPoint targetPoint in validTargets)
            {
                RaycastHit hit;
                Physics.SphereCast(targetPoint.TargetPosition, targetPoint.occupationRad, Vector3.zero, out hit, LayerMask.NameToLayer("Enemy"));
                if (hit.collider != null)
                {
                    Debug.Log("Occupied target Point");
                    validTargets.Remove(targetPoint);
                }
            }

        }

        if (filters.Occupation)
        {
            foreach (EnvTargetPoint targetPoint in validTargets)
            {
                if (targetPoint.IsOccupied)
                    validTargets.Remove(targetPoint);
            }
        }

        return validTargets.ToArray();
    }

    #endregion

}
