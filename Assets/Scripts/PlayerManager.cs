using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPrefab;
    [SerializeField] private FighterList _selectedFighters;

    private PlayerInput _input;

    private GameObject _currentPlayerObject;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        DontDestroyOnLoad(gameObject);
        _currentPlayerObject = Instantiate(_menuPrefab);

        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        var player = Instantiate(_selectedFighters.Fighters[_input.playerIndex].FighterPrefab);
        _currentPlayerObject = player;
    }

    private void OnMovement()
    {
        _currentPlayerObject?.SendMessage("OnMovement");
    }

    private void OnVertical()
    {
        _currentPlayerObject?.SendMessage("OnVertical");
    }

    private void OnPunch()
    {
        _currentPlayerObject?.SendMessage("OnPunch");
    }

    private void OnKick()
    {
        _currentPlayerObject?.SendMessage("OnKick");
    }

    private void OnMoveSelection()
    {
        Debug.Log("Moving selection calling on " + _currentPlayerObject.name);
        _currentPlayerObject?.SendMessage("OnMoveSelection");
    }

    private void OnConfirm()
    {
        _currentPlayerObject?.SendMessage("OnConfirm");
    }

    private void OnBack()
    {
        _currentPlayerObject?.SendMessage("OnBack");
    }

    private void OnSpecial()
    {
        _currentPlayerObject?.SendMessage("OnSpecial");
    }
}
