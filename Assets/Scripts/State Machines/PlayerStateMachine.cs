using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 8f;
    public Transform Facing { get; private set; }
    public float FacingSign { get; private set; } = 1f;

    public Animator anim { get; private set; }
    public CharacterController controller { get; private set; }
    public PlayerInput input { get; private set; }

    private static uint _playerCount = 0;
    private static PlayerStateMachine[] _players;

    private bool _hasFlipped = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        _playerCount++;
        _players ??= new PlayerStateMachine[2];
        _players[_playerCount - 1] = this;

        if (_playerCount == 1)
        {
            transform.position = GameObject.Find("Spawn1").transform.position;
            GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>()
                .RemoveMember(GameObject.Find("Spawn1").transform);
        }
        else
        {
            transform.position = GameObject.Find("Spawn2").transform.position;
            transform.eulerAngles = new Vector3(0, -90f, 0);
            _players[0].Facing = _players[1].transform;
            _players[1].Facing = _players[0].transform;
            GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>()
                .RemoveMember(GameObject.Find("Spawn2").transform);
        }
        GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>().AddMember(transform, 1, 0);

        SwitchState(new PlayerMoveState(this));
    }

    private void OnVertical()
    {
        if (controller.isGrounded)
            SwitchState(new PlayerJumpState(this));
    }

    private void OnPunch()
    {
        if (GetCurrentState().GetType() == typeof(PlayerMoveState))
            SwitchState(new PlayerAttackState(this));
    }

    public void StopAttack()
    {
        SwitchState(new PlayerMoveState(this));
    }

    public void SwitchFacingDirection()
    {
        if (Facing == null) return;
        if (_hasFlipped) { _hasFlipped = false; return; }

        _hasFlipped = true;
        transform.eulerAngles = transform.position.z < Facing.position.z ?
            new Vector3(0, 90, 0) : new Vector3(0, -90, 0);
        FacingSign = transform.position.z < Facing.position.z ? 1 : -1;
        Facing.GetComponent<PlayerStateMachine>().SwitchFacingDirection();
    }
}
