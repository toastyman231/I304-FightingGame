using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameOverUIController : MonoBehaviour
{
    public static event Action<int> MoveSelection;
    public static event Action ConfirmSelection;
    public static int NumButtons;

    private UIDocument _document;

    private Button _selectedButton;
    private List<Button> _buttons;

    private void OnDestroy()
    {
        MoveSelection -= OnMoveSelection;
        ConfirmSelection -= OnConfirmSelection;

        _buttons[0].clicked -= OnRematch;
        _buttons[1].clicked -= OnNewFight;
        _buttons[2].clicked -= OnQuit;
    }

    private void SelectButton(Button button)
    {
        _selectedButton?.RemoveFromClassList("selected-button");
        _selectedButton = button;
        _selectedButton.AddToClassList("selected-button");
    }

    private void OnRematch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNewFight()
    {
        SceneManager.LoadScene((int)Scenes.MAIN, LoadSceneMode.Single);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnMoveSelection(int item)
    {
        SelectButton(_buttons[item]);
    }

    private void OnConfirmSelection()
    {
        using var e = new NavigationSubmitEvent() { target = _selectedButton };
        _selectedButton.SendEvent(e);
    }

    public void SetupGameOver(string victorName)
    {
        _document = GetComponent<UIDocument>();
        _buttons = _document.rootVisualElement.Query<Button>().ToList();
        NumButtons = _buttons.Count;

        _buttons[0].clicked += OnRematch;
        _buttons[1].clicked += OnNewFight;
        _buttons[2].clicked += OnQuit;

        MoveSelection += OnMoveSelection;
        ConfirmSelection += OnConfirmSelection;

        var root = _document.rootVisualElement;
        root.Q<Label>("Message").text = victorName.ToUpper() + " WINS!";
        SelectButton(_buttons[0]);
    }

    public static void InvokeMoveSelection(int item)
    {
        MoveSelection?.Invoke(item);
    }

    public static void InvokeConfirmSelection()
    {
        ConfirmSelection?.Invoke();
    }
}
