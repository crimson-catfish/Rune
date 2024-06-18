using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneTogglesContainer : MonoBehaviour
{
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private GameObject runeTogglePrefab;
    [SerializeField] private RuneStorage storage;
    [SerializeField] private ToggleGroup toggleGroup;


    private Vector2 scrollPosition;
    private readonly Dictionary<int, Toggle> toggles = new();
    private Rune currentRecognized;

    private void OnEnable()
    {
        recognizer.OnRuneRecognized += rune => currentRecognized = rune;
    }


    private void Start()
    {
        toggleGroup = this.GetComponent<ToggleGroup>();
        foreach (Rune rune in storage.Runes.Values)
            AddToggle(rune);
    }

    private void OnDisable()
    {
        recognizer.OnRuneRecognized -= rune => currentRecognized = rune;
    }

    public void AddNewToggle(Rune rune)
    {
        AddToggle(rune).isOn = true;
    }

    public void RemoveToggle(Rune rune)
    {
        toggles.Remove(rune.GetHashCode());
        toggleGroup.GetFirstActiveToggle().gameObject.SetActive(false);
        toggleGroup.SetAllTogglesOff();
    }


    public void SelectRecognizedRune()
    {
        if (currentRecognized == null)
            return;

        if (toggles.TryGetValue(currentRecognized.GetHashCode(), out Toggle toggle))
            toggle.isOn = true;
    }

    private Toggle AddToggle(Rune rune)
    {
        GameObject runeToggleObject = Instantiate(runeTogglePrefab, this.transform);

        if (runeToggleObject.TryGetComponent(out Toggle toggle))
            toggle.group = toggleGroup;

        if (runeToggleObject.TryGetComponent(out RuneToggle runeToggle))
            runeToggle.Rune = rune;

        if (runeToggleObject.TryGetComponent(out AspectRatioFitter ratioFitter))
            ratioFitter.aspectRatio = (float)rune.Preview.width / (float)rune.Preview.height;

        toggles.Add(rune.GetHashCode(), toggle);

        return runeToggleObject.GetComponent<Toggle>();
    }
}