using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimatorParameterType
{
    INTEGER,
    FLOAT,
    BOOL,
    TRIGGER,
}

public enum AnimatorCharacterType
{
    NoWeapon = 0,
    Ranged = 1,
    Melee = 2,
}

public struct AnimatorParameterStats
{
    public string parameterName;

    public AnimatorParameterType Type;
    public int IntegerValue;
    public float FloatValue;
    public bool BoolValue;

    public AnimatorParameterStats(string parameterName, AnimatorParameterType type, int intValue)
    {
        this.parameterName = parameterName;
        this.Type = type;

        IntegerValue = intValue;
        FloatValue = 0;
        BoolValue = false;
    }

    public AnimatorParameterStats(string parameterName, AnimatorParameterType type, float floatValue)
    {
        this.parameterName = parameterName;
        this.Type = type;

        IntegerValue = 0;
        FloatValue = floatValue;
        BoolValue = false;
    }

    public AnimatorParameterStats(string parameterName, AnimatorParameterType type, bool boolValue)
    {
        this.parameterName = parameterName;
        this.Type = type;

        IntegerValue = 0;
        FloatValue = 0;
        BoolValue = boolValue;
    }
}