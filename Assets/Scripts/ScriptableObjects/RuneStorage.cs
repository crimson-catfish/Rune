using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [property: SerializeField] public HashSet<Rune> Runes { get; private set; } = new();


    private void OnEnable()
    {
        Runes.AddRange(Resources.LoadAll<Rune>("Runes"));
    }


    public void NewRune()
    {
        Rune rune = CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(new Texture2D(128, 128), rune.previewPath);
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        Runes.Add(rune);
    }

    public void DeleteRune(Rune rune)
    {
        Runes.Remove(rune);
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
    }
}