using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerStateMachine : StateMachine, IDamageable
{
    public float Health;
    public float MaxHealth = 100f;
    public Vector3 Velocity;
    public bool IFrame = false;
    public AttackInfo[] attacks;
    public float MovementSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 8f;
    public Transform Facing { get; private set; }
    public float FacingSign { get; private set; } = 1f;

    public Animator anim { get; private set; }
    public CharacterController controller { get; private set; }
    public PlayerInput input { get; private set; }
    public bool hasControl { get; private set; } = true;

    private static uint _playerCount = 0;
    private static PlayerStateMachine[] _players;

    private bool _hasFlipped = false;
    private bool _isCrouching = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        _playerCount++;
        if (_playerCount > 2) _playerCount = 1;
        _players ??= new PlayerStateMachine[2];
        _players[_playerCount - 1] = this;

        Health = MaxHealth;
        FighterUIController.InvokeHealthChanged((uint)input.playerIndex, Health, MaxHealth);
        FighterUIController.GameOver += OnGameOver;

        Setup();

        SwitchState(new PlayerMoveState(this));
    }

    private void OnDestroy()
    {
        FighterUIController.GameOver -= OnGameOver;
    }

    private void Setup()
    {
        if (input.playerIndex == 0)
        {
            GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>()
                .RemoveMember(GameObject.Find("Spawn1").transform);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -90f, 0);
            _players[0].Facing = _players[1].transform;
            _players[1].Facing = _players[0].transform;
            GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>()
                .RemoveMember(GameObject.Find("Spawn2").transform);
        }
        GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>().AddMember(transform, 1, 0);
    }

    private void OnGameOver(uint victorId)
    {
        hasControl = false;
        input.currentActionMap = input.actions.FindActionMap("Menu", true);
    }

    private void OnVertical()
    {
        if (!controller.isGrounded || !hasControl) return;

        float verticalValue = input.actions["Vertical"].ReadValue<float>();
        if (verticalValue > 0)
        {
            if (_isCrouching)
            {
                _isCrouching = false;
                anim.SetBool("IsCrouching", false);
                SwitchState(new PlayerMoveState(this));
            }
            else
            {
                _isCrouching = false;
                SwitchState(new PlayerJumpState(this));
            }
        } else if (verticalValue < 0)
        {
            if (_isCrouching) return;

            _isCrouching = true;
            SwitchState(new PlayerCrouchState(this));
        }
    }

    private void OnPunch()
    {
        if (!hasControl) return;

        if (GetCurrentState().GetType() == typeof(PlayerMoveState))
            SwitchState(new PlayerAttackState(this, attacks[0]));
    }

    private void OnKick()
    {
        if (!hasControl) return;

        if (GetCurrentState().GetType() == typeof(PlayerMoveState))
            SwitchState(new PlayerAttackState(this, attacks[1]));
    }

    public void StopAttack()
    {
        SwitchState(new PlayerMoveState(this));
    }

    public void EndDamageState()
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

    public void ReceiveDamage(float amount, DamageType damageType)
    {
        if (IFrame) return;

        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        FighterUIController.InvokeHealthChanged((uint)input.playerIndex, Health, MaxHealth);
        SwitchState(new PlayerDamageState(this, damageType));

        if (Health <= 0)
        {
            FighterUIController.InvokeGameOver((input.playerIndex == 0) ? 1U : 0U);
        }
    }

    public override void OnAnyStateEnter()
    {
        return;
    }

    public override void OnAnyStateTick()
    {
        return;
    }

    public override void OnAnyStateExit()
    {
        return;
    }
}
