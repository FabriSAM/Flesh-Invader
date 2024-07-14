using System;
using UnityEngine;

public struct Collectible
{
    public CollectiblesFound collectiblesFound;
    public CollectibleInfo Info;
}

[Serializable]
public struct CollectibleInfo
{
    public string Title;
    public Texture2D Icon;
}

[Serializable]
public struct CollectiblesFound {
    public int MaxObject;
    public int CurrentObject;
}
