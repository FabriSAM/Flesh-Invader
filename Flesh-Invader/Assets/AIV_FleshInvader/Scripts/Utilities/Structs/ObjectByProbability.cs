using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjectByProbability<T>
{
    [SerializeField] public float Min;
    [SerializeField] public float Max;
    [SerializeField] public T result;

    public bool IsInRange(float value)
    {
        return (value >= Min && value <= Max);
    }
}