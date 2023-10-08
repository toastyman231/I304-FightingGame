using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFighterList", menuName = "Scriptable Objects/Fighter List")]
public class FighterList : ScriptableObject
{
    public List<FighterData> Fighters;
}
