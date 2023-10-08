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

    private void OnPlayerJoined(PlayerInput input)
    {
        if (input.playerIndex == 0)
        {
            input.transform.position = GameObject.Find("Spawn1").transform.position;
        }
        else
        {
            input.transform.position = GameObject.Find("Spawn2").transform.position;
        }
    }
}
