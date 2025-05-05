using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour, IPlayerMoveContext, IPlayerJumpContext
{
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    public float _walkSpeed = 10f;
    public float _jumpForce = 5f;
    public float _fallMultiplier = 2.5f;
    private float _velocity;

    public Transform Transform => transform;
    public Vector2 Input => new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    public float Speed => _walkSpeed;
    public float JumpForce => _jumpForce;
    public bool IsGrounded => CharacterController.isGrounded;
    public float GetVerticalVelocity() => _velocity;
    public void SetVerticalVelocity(float value) => _velocity = value;


    private PlayerStateMachine _stateMachine;
    private IPlayerJumpStrategy jumpStrategy = new PlayerJumpStrategy();
    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        _stateMachine = new PlayerStateMachine(this);

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
}
