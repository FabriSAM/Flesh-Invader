using System;
using UnityEngine;

public struct Collectible
{
    public int MaxObject;
    public int CurrentObject;

    public CollectibleInfo Info;
}

[Serializable]
public struct CollectibleInfo
{
    public string Title;
    public Texture2D Icon;
}
