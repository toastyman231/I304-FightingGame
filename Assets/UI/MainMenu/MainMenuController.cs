using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private StyleSheet style;
    [SerializeField] private FighterList fighters;

    private UIDocument _document;

    private int _p1Selected = -1;
    private int _p2Selected = -1;
    private List<VisualElement> _selectors;
    private List<GameObject> _fighterPreview;

    private readonly string SELECTED_P1 = "fighter-selector-selectedP1";
    private readonly string SELECTED_P2 = "fighter-selector-selectedP2";

    public static event Action<uint, uint> MoveSelection;
    public static event Action<uint> SelectFighter;
    public static event Action<uint> UnselectFighter;
    public static int Rows = 1;
    public static int Columns = 2;

    private void Start()
    {
        _fighterPreview = new List<GameObject>(2);
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;
        root.styleSheets.Add(style);
        _selectors = root.Query<VisualElement>("FighterSelector").ToList();
        Rows = root.Q<VisualElement>("FightersHolder").childCount - 1;
        Columns = root.Q<VisualElement>("FightersRow1").childCount;

        int index = 0;
        foreach (var selector in _selectors)
        {
            if (index > fighters.Fighters.Count-1) continue;

            var fighter = fighters.Fighters[index];
            selector.Q<Label>().text = fighter.FighterName;
        }

        MoveSelection += OnMoveSelection;
        SelectFighter += OnFighterSelected;
        UnselectFighter += OnFighterUnselected;
    }

    private void OnDestroy()
    {
        MoveSelection -= OnMoveSelection;
        SelectFighter -= OnFighterSelected;
        UnselectFighter -= OnFighterUnselected;
    }

    private void OnMoveSelection(uint player, uint index)
    {
        if (player == 0)
        {
            SetP1Selected((int)index);
        }
        else
        {
            SetP2Selected((int)index);
        }
    }

    private void SetP1Selected(int index)
    {
        RemoveAllSelectors();

        _p1Selected = index;
        if (_p1Selected == _p2Selected)
        {
            _selectors[_p1Selected].AddToClassList(SELECTED_P1);
            _selectors[_p1Selected].parent.AddToClassList(SELECTED_P2);
        }
        else
        {
            _selectors[_p1Selected].AddToClassList(SELECTED_P1);
            if (_p2Selected >= 0) _selectors[_p2Selected].AddToClassList(SELECTED_P2);
        }
    }

    private void SetP2Selected(int index)
    {
        RemoveAllSelectors();

        _p2Selected = index;
        if (_p1Selected == _p2Selected)
        {
            _selectors[_p2Selected].AddToClassList(SELECTED_P1);
            _selectors[_p2Selected].parent.AddToClassList(SELECTED_P2);
        }
        else
        {
            if (_p1Selected >= 0) _selectors[_p1Selected].AddToClassList(SELECTED_P1);
            _selectors[_p2Selected].AddToClassList(SELECTED_P2);
        }
    }

    private void OnFighterSelected(uint player)
    {
        var index = (player == 0) ? _p1Selected : _p2Selected;
        var position = (player == 0) ? "Fighter1Pos" : "Fighter2Pos";

        var fighter = Instantiate(fighters.Fighters[index].FighterPreview);
        fighter.transform.position = GameObject.Find(position).transform.position;
        if (player > 0) fighter.transform.eulerAngles = Vector3.zero;
        _fighterPreview.Add(fighter);
    }

    private void OnFighterUnselected(uint player)
    {
        Destroy(_fighterPreview[(int)player]);
    }

    private void RemoveAllSelectors()
    {
        if (_p1Selected >= 0)
        {
            _selectors[_p1Selected].RemoveFromClassList(SELECTED_P1);
            _selectors[_p1Selected].parent.RemoveFromClassList(SELECTED_P1);
        }

        if (_p2Selected >= 0)
        {
            _selectors[_p2Selected].RemoveFromClassList(SELECTED_P2);
            _selectors[_p2Selected].parent.RemoveFromClassList(SELECTED_P2);
        }
    }

    public static void MoveSelector(uint player, uint index)
    {
        MoveSelection?.Invoke(player, index);
    }

    public static void InvokeSelectFighter(uint player)
    {
        SelectFighter?.Invoke(player);
    }

    public static void InvokeUnselectFighter(uint player)
    {
        UnselectFighter?.Invoke(player);
    }
}
