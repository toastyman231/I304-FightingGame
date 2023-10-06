using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTypePicker : MonoBehaviour
{
    [SerializeField] private DamageType damageType;

    public DamageType PickDamageType()
    {
        return damageType;
    }
}
