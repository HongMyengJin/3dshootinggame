using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, IPlayerMoveContext, IPlayerJumpContext, IPlayerDamageContext
{
    protected PlayerStateType _sheduledChangeType;
    public CharacterController CharacterController { get; private set; }
    public Action<Damage> OnDamageReceived;
    public Animator Animator { get; private set; }
    public float _walkSpeed = 10f;
    public float _jumpForce = 5f;
    public float _fallMultiplier = 2.5f;
    private float _velocity;
    private Vector3 _knockbackDirection;

    public Transform Transform => transform;
    public Vector2 Input => new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    public float Speed => _walkSpeed;
    public float JumpForce => _jumpForce;
    public bool IsGrounded => CharacterController.isGrounded;
    public Vector3 KnockbackDirection => _knockbackDirection;
    public float GetVerticalVelocity() => _velocity;
    public void SetVerticalVelocity(float value) => _velocity = value;

    private PlayerStateMachine _stateMachine;
    private IPlayerJumpStrategy jumpStrategy = new PlayerJumpStrategy();
    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        _stateMachine = new PlayerStateMachine(this, Animator);

        PlayerStat playerStat = GetComponent<PlayerStat>();
        playerStat.OnStateChangeRequested += _stateMachine.ChangeState;

        jumpStrategy.OnJumpPerformed += () =>
        {
            Animator.SetBool("Jump", true);                // 점프 중 상태 표시
        };
    }

    private void Update()
    {
        _stateMachine.Update();
        HandleJumpInput();
        ApplyGravity();
    }

    private void HandleJumpInput()
    {
        if (InputManager.Instance.JumpPressed)
        {
            if (!IsGrounded)
                return;
            jumpStrategy.Jump(this);
            InputManager.Instance.ResetJump(); // 점프 리셋
        }
    }
    private void ApplyGravity()
    {
        if (IsGrounded && _velocity < 0)
        {
            Animator.SetBool("Jump", false);
            // _velocity = -1f;
        }
        else
        {   

            _velocity += Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            Animator.SetFloat("yVelocity", _velocity);  // 점프 순간 속도 등록
        }
        
    }

    public void ScheduleStateChange(PlayerStateType next, float delay = 0.0f)
    {
        StartCoroutine(DelayedChange(next, delay));
    }

    private IEnumerator DelayedChange(PlayerStateType next, float delay)
    {
        _sheduledChangeType = next;
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(next);
    }

    public void TakeDamage(Damage data)
    {
        OnDamageReceived?.Invoke(data);
    }
}
