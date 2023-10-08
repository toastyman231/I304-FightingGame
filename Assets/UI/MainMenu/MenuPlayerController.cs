using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class MenuPlayerController : MonoBehaviour
{
    [SerializeField] private FighterList _fighters;
    [SerializeField] private FighterList _selectedFighters;

    private PlayerInput _input;

    private uint _index = 0;
    private int _selection = 0;
    private bool _selected = false;
    private bool _controlEnabled = false;

    private static uint _playerCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _playerCount++;

        if (_playerCount > 1)
        {
            _index = 1;
            _selection = 1;
            MainMenuController.MoveSelector(1, 1);
        }
        else
        {
            MainMenuController.MoveSelector(0, 0);
        }

        StartCoroutine(EnableControlAfterSeconds(0.2f));
    }

    private IEnumerator EnableControlAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        _controlEnabled = true;
    }

    private void OnMoveSelection()
    {
        if (_selected || !_controlEnabled) return;

        var value = _input.actions["MoveSelection"].ReadValue<Vector2>();

        if (value.x != 0)
        {
            _selection += (value.x > 0) ? Mathf.CeilToInt(value.x) : Mathf.FloorToInt(value.x);
            if (_selection > MainMenuController.Columns - 1)
                _selection = 0;
            else if (_selection < 0)
                _selection = MainMenuController.Columns - 1;
        } else if (value.y != 0)
        {
            // TODO: Add support for moving between UI rows
        }

        MainMenuController.MoveSelector(_index, (uint)_selection);
    }

    private void OnConfirm()
    {
        if (!_controlEnabled) return;

        _selected = true;
        MainMenuController.InvokeSelectFighter(_index);
    }

    private void OnBack()
    {
        if (!_controlEnabled) return;

        _selected = false;
        MainMenuController.InvokeUnselectFighter(_index);
    }

    private void OnSpecial()
    {
        if (!_controlEnabled || _playerCount < 2) return;

        var players = GameObject.FindObjectsByType<MenuPlayerController>(FindObjectsSortMode.None);
        _selectedFighters.Fighters.Clear();

        foreach (var player in players)
        {
            _selectedFighters.Fighters.Add(player._fighters.Fighters[player._selection]);
        }

        SceneManager.LoadScene((int)Scenes.DEMO, LoadSceneMode.Single);
    }
}
