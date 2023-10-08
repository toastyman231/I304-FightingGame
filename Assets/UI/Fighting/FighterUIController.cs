using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FighterUIController : MonoBehaviour
{
    [SerializeField] private FighterList _selectedFighters;

    private UIDocument _document;

    private CustomProgressBar _p1HealthBar;
    private CustomProgressBar _p2HealthBar;

    public static event Action<uint, float, float> HealthChanged;

    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;

        _p1HealthBar = root.Q<CustomProgressBar>("Fighter1Health");
        _p2HealthBar = root.Q<CustomProgressBar>("Fighter2Health");

        root.Q<Label>("Fighter1Name").text = _selectedFighters.Fighters[0].FighterName;
        root.Q<Label>("Fighter2Name").text = _selectedFighters.Fighters[1].FighterName;

        root.Q<VisualElement>("Fighter1Icon").style.backgroundImage =
            new StyleBackground(_selectedFighters.Fighters[0].FighterImage);
        root.Q<VisualElement>("Fighter2Icon").style.backgroundImage =
            new StyleBackground(_selectedFighters.Fighters[1].FighterImage);

        HealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(uint player, float current, float max)
    {
        var healthBar = (player == 0) ? _p1HealthBar : _p2HealthBar;
        healthBar.value = current / max;
    }

    public static void InvokeHealthChanged(uint player, float current, float max)
    {
        HealthChanged?.Invoke(player, current, max);
    }
}
