using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDummy : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDamage(float amount)
    {
        Debug.Log("Hit for " + amount + " damage!");
    }
}
