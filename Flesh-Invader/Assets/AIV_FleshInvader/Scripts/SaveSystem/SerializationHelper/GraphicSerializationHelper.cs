using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NamedColor
{
    public string name;
    public Color color;

    public bool ColorHasName(string name)
    {
        return (name == this.name);
    }
}

[Serializable]
public struct NamedTexture2D
{
    public string name;
    public Texture2D texture;

    public bool TextureHasName(string name)
    {
        return (name == this.name);
    }
}

[CreateAssetMenu(fileName = "Graphic_SerializationHelper", menuName = "SerializationHelpers/GraphicSerializationHelper", order = 0)]
public class GraphicSerializationHelper : ScriptableObject
{
    [SerializeField] public List<NamedColor> nameToColorAssetMap;

    [SerializeField] public List<NamedTexture2D> nameToTextureAssetMap;
}
