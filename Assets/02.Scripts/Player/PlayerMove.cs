using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    // 목표: wasd를 누르면 캐릭터를 카메라 방향에 맞게 이동시키고 싶다.
    // 필요 속성:
    // - 이동속도
    public float MoveSpeed = 7f;
    public float JumpPower = 10f;

    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f;       // 중력가속도

    private CharacterController _characterController;


    public float CurStamina = 0.0f;
    public float MaxStamina = 100.0f;
    
    private const float _staminaUseSpeed = 20.0f; // 스태미나 소모 속도 - 뛰기
    private const float _staminaFillSpeed = 40.0f; // 스태미나 채워지는 속도

    private float _staminaDashUseValue = 30.0f; // 스태미나 소모량 - 대쉬

    private int JumpN = 0;

    private bool _bDash = false;

    private Vector3 Dir = Vector3.zero;

    private bool _bClimb = false; // 벽타기 여부

    // 구현 순서:
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    public Slider Slider;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        CurStamina = MaxStamina;
        ChangeStamina();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (Mathf.Abs(dir.x) > 0.0f || Mathf.Abs(dir.z) > 0.0f)
            Dir = dir;
        dir = Camera.main.transform.TransformDirection(dir);

        if (!Climb(dir, h, v))
            return;

        Jump();

        if (Input.GetKey(KeyCode.LeftShift) && CurStamina > 0.0f){
            Run();
        }
        else if (Input.GetKeyDown(KeyCode.E)){
            Dash();
        }
        else if(!_bDash){
            BasicStamina();
        }

        ChangeStamina();

        dir = GetVelocity(dir);
        if (_bDash)
            return;

        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    public Vector3 GetVelocity(Vector3 dir)
    {
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        return dir;
    }

    public void ChangeStamina()
    {
        Slider.value = CurStamina / MaxStamina;
    }

    IEnumerator CoDash(float dashTime)
    {
        float elapseTime = 0.0f;
        while (elapseTime < dashTime)
        {
            elapseTime += Time.deltaTime;
            _characterController.Move(transform.forward * MoveSpeed * 5.0f * Time.deltaTime);
            yield return null;
        }

        _bDash = false;
    }


    public bool Climb(Vector3 dir, float h, float v)
    {
        if (_bClimb) // 벽 타기 중이면
        {
            dir = new Vector3(h, v, 0).normalized;
            dir = Camera.main.transform.TransformDirection(dir);
            dir.z = 0.0f;
            _characterController.Move(dir * MoveSpeed * Time.deltaTime);
            CurStamina -= _staminaUseSpeed * Time.deltaTime;

            _yVelocity = 0.0f; // 중력 초기화
            if (CurStamina < 0.0f)
                _bClimb = false;
            ChangeStamina();
            return false;
        }

        return true;
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            JumpN = 0;
        }

        // 3. 점프 적용
        if (Input.GetButtonDown("Jump") && JumpN < 2)
        {
            _yVelocity = JumpPower;

            JumpN++;
        }
    }

    public void Run()
    {
        MoveSpeed = 12.0f;
        CurStamina -= Time.deltaTime * _staminaUseSpeed;
        if (CurStamina < 0.0f)
            CurStamina = 0.0f;
    }

    public void Dash()
    {
        if (_bDash == false)
        {
            if (CurStamina - _staminaDashUseValue < 0)
                return;

            CurStamina -= _staminaDashUseValue;
            _bDash = true;
            Dir = Camera.main.transform.TransformDirection(Dir);
            StartCoroutine(CoDash(0.5f));
        }
    }

    public void BasicStamina()
    {
        MoveSpeed = 7.0f;

        if (CurStamina < MaxStamina)
            CurStamina += Time.deltaTime * _staminaFillSpeed;
        else
            CurStamina = MaxStamina;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
            _bClimb = true;
    }
}

