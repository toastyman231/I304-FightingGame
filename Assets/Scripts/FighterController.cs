using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class FighterController : MonoBehaviour
{
    private Animator _anim;
    private PlayerInput _input;
    private Rigidbody _rb;
    private CharacterController _controller;

    private bool _isMoving = false;
    private bool _canMove = true;
    private bool _canAttack = true;
    private bool _canJump = true;
    private bool _isCrouching = false;
    private Vector3 _moveDirection = Vector3.zero;

    private float _movementScale = 1;

    [SerializeField] private float walkSpeed = 25;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float damage = 10;
    [SerializeField] private float groundDistance = 0.01f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform facing;

    // Start is called before the first frame update
    void Start()
    {
        if (groundCheck == null) groundCheck = GameObject.Find("GroundCheck").transform;
        _anim = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();

        FighterController[] players = GameObject.FindObjectsOfType<FighterController>();
        if (players.Length == 1)
        {
            transform.position = GameObject.Find("Spawn1").transform.position;
        }
        else
        {
            transform.position = GameObject.Find("Spawn2").transform.position;
            transform.eulerAngles = new Vector3(0, -90f, 0);
            players[0].facing = players[1].transform;
            players[1].facing = players[0].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _movementScale = (_canMove) ? 1 : 0;

        if (!_controller.isGrounded)
        {
            _moveDirection -= (Vector3.down * Physics.gravity.y * Time.deltaTime);
            if (_controller.velocity.y < 0) _anim.SetBool("IsFalling", true);
        }
        else
        {
            Debug.Log("Grounded");
            _anim.SetBool("IsFalling", false);
            _anim.SetTrigger("Land");
            _canAttack = true;
            StartCoroutine(EndJumpCallback());
        }

        Debug.Log(_moveDirection);
        _controller.Move(_moveDirection * _movementScale * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            IDamageable[] damageables = other.gameObject.GetComponents<IDamageable>();

            foreach (IDamageable damageable in damageables)
            {
                damageable.ReceiveDamage(damage);
            }
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawLine(groundCheck.position, groundCheck.position + (Vector3.down * groundDistance), Color.red, 2f);
        if (Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer))
        {
            return true;
        }

        return false;
    }

    public void StopAttack()
    {
        _canMove = true;
        _canAttack = true;
        _canJump = true;
    }

    public void EndJump()
    {
        return;

        _rb.drag = 100;
        _canAttack = true;
        if (facing != null)
        {
            transform.eulerAngles = transform.position.z < facing.position.z ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0);
        }
        StartCoroutine(EndJumpCallback());
    }

    private IEnumerator EndJumpCallback()
    {
        yield return new WaitForSeconds(0.05f);
        _canJump = true;
    }

    private void OnMovement()
    {
        float value = _input.actions["Movement"].ReadValue<float>();
        _anim.SetFloat("WalkDirection", value);
        _moveDirection = Vector3.forward * value * walkSpeed;
        _isMoving = value != 0;
    }

    private void OnPunch()
    {
        if (!_canAttack) return;

        _canAttack = false;
        _canMove = false;
        _canJump = false;
        _anim.SetTrigger("Punch");
    }

    private void OnVertical()
    {
        if (!_canJump) return;

        float value = _input.actions["Vertical"].ReadValue<float>();
        if (value > 0 && _controller.isGrounded)
        {
            if (_isCrouching)
            {
                _anim.SetBool("IsCrouching", false);
                _isCrouching = false;
                return;
            }

            _canJump = false;
            _canAttack = false;
            _moveDirection.y = jumpForce; // Alternate movement code: Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            _anim.SetBool("IsCrouching", false);
            _anim.SetTrigger("Jump");
        }
        else if (value < 0)
        {
            _anim.SetBool("IsCrouching", true);
            _isCrouching = true;
        }
    }
}
