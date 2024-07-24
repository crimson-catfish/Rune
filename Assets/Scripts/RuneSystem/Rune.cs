using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
///     rune's width always 1, height is positive float.
/// </summary>
[Serializable]
public class Rune : ScriptableObject, IComparable<Rune>
{
    public const float Width = 1;

    public string                  previewPath;
    public float                   averageHeight;
    public Vector2                 averageMassCenter = Vector2.zero;
    public List<RuneDrawVariation> drawVariations    = new();

#if UNITY_EDITOR
    public Texture2D Preview {
        get
        {
            if (preview == null)
                preview = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);

            return preview;
        }
        set => preview = value;
    }
#endif

    private Texture2D preview;

    public int CompareTo(Rune other)
    {
        if (drawVariations.GetHashCode() < other.drawVariations.GetHashCode()) return -1;
        if (drawVariations.GetHashCode() > other.drawVariations.GetHashCode()) return 1;

        return 0;
    }
}