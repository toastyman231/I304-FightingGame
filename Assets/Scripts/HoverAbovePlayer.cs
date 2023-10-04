using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAbovePlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private PlayerStateMachine _player;

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindObjectOfType<PlayerStateMachine>();
            return;
        }

        transform.position = _player.transform.position + offset;
    }
}
