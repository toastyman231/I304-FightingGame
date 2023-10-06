using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyAttackInfo", menuName = "Scriptable Objects/Attack Info")]
public class AttackInfo : ScriptableObject
{
    public string AttackName;

    public string AttackTrigger;

    public float AttackDamage;
}
