using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class SetupFight : MonoBehaviour
{
    [SerializeField] private FighterList _selectedFighters;

    private PlayerInputManager _inputManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var fighter in _selectedFighters.Fighters)
        {
            Instantiate(fighter.FighterPrefab);
        }
    }
}
