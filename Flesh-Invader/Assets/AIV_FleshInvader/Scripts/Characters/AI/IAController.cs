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

    public EnvTargetPoint() : this(Vector3.zero, 0) { }


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
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private int targetNumber;
    #endregion

    public IAController()
    {
        targetPoints = new EnvTargetPoint[targetNumber];
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
            //Physics.SphereCast(targetPoint.TargetPosition, targetPoint.occupationRad);
            
        }
        if (filters.Occupation)
        {
            foreach (EnvTargetPoint targetPoint in validTargets)
            {
                RaycastHit hit;
                Physics.SphereCast(targetPoint.TargetPosition, targetPoint.occupationRad, Vector3.zero, out hit, LayerMask.NameToLayer("Enemy"));
                if(hit.collider != null)
                {
                    Debug.Log("Occupied target Point");
                    validTargets.Remove(targetPoint);
                }
            }
        }

        return validTargets.ToArray();
    }

    #endregion

}