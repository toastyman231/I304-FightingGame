using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] private AttackInfo attack;
    [SerializeField] private int numHits = 1;
    [SerializeField] private int[] maxHitsPerHit;

    private int _currentHit = 0;

    private void Start()
    {
        if (maxHitsPerHit == null)
        {
            maxHitsPerHit = new int[numHits];
            Array.Fill(maxHitsPerHit, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentHit >= numHits || other.transform.root == transform.root) return;

        Debug.Log("Triggered by " + other.gameObject.name);
        DamageType damageType = other.GetComponent<CollisionTypePicker>()?.PickDamageType() ?? DamageType.HEAD;
        IDamageable[] damageables = other.transform.root.gameObject.GetComponents<IDamageable>();

        for (int i = 0; i < maxHitsPerHit[_currentHit]; i++)
        {
            if (i < 0 || i > damageables.Length - 1) break;

            damageables[i].ReceiveDamage(attack.AttackDamage, damageType);
        }

        _currentHit++;
    }

    public void Reset()
    {
        _currentHit = 0;
    }
}
