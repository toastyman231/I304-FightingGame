using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyFigherData", menuName = "Scriptable Objects/Fighter Data")]
public class FighterData : ScriptableObject
{
    public string FighterName;

    public Sprite FighterImage;

    public GameObject FighterPrefab;

    public GameObject FighterPreview;
}
