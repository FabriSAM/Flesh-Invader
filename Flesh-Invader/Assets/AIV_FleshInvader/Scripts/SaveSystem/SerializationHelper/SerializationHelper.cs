using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationHelper : MonoBehaviour
{
    [SerializeField] private GraphicSerializationHelper graphicHelper;

    public static SerializationHelper instance;
    public static SerializationHelper Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<SerializationHelper>();
        }
        return instance;
    }

    public Color GetColorByName(string colorName)
    {
        foreach (NamedColor namedColor in graphicHelper.nameToColorAssetMap)
        {
            if (namedColor.name == colorName)
                return namedColor.color;
        }
        return Color.clear;
    }

    public Texture2D GetTextureByName(string colorName)
    {
        foreach (NamedTexture2D namedTexture in graphicHelper.nameToTextureAssetMap)
        {
            if (namedTexture.name == colorName)
                return namedTexture.texture;
        }
        return null;
    }
}
