using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameOverMenuController : MonoBehaviour
{
    private PlayerInput _input;
    private int _selection = 0;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void OnMoveSelection()
    {
        if (_input.playerIndex > 0) return;

        var value = _input.actions["MoveSelection"].ReadValue<Vector2>();

        if (value.y > 0)
        {
            _selection--;
        } else if (value.y < 0)
        {
            _selection++;
        }

        if (value.y != 0)
        {
            if (_selection > GameOverUIController.NumButtons - 1)
                _selection = 0;
            else if (_selection < 0)
                _selection = GameOverUIController.NumButtons - 1;

            GameOverUIController.InvokeMoveSelection(_selection);
        }
    }

    private void OnConfirm()
    {
        if (_input.playerIndex > 0) return;

        GameOverUIController.InvokeConfirmSelection();
    }
}
