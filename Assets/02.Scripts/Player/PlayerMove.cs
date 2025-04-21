using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ: wasd�� ������ ĳ���͸� ī�޶� ���⿡ �°� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 7f;
    public float JumpPower = 10f;

    private const float GRAVITY = -9.8f; // �߷�
    private float _yVelocity = 0f;       // �߷°��ӵ�

    private CharacterController _characterController;


    public float CurStamina = 0.0f;
    public float MaxStamina = 100.0f;
    
    private const float _staminaUseSpeed = 20.0f; // ���¹̳� �Ҹ� �ӵ� - �ٱ�
    private const float _staminaFillSpeed = 40.0f; // ���¹̳� ä������ �ӵ�

    private float _staminaDashUseValue = 30.0f; // ���¹̳� �Ҹ� - �뽬

    private int JumpN = 0;

    private bool _bDash = false;

    private Vector3 Dir = Vector3.zero;

    private bool _bClimb = false; // ��Ÿ�� ����

    // ���� ����:
    // 1. Ű���� �Է��� �޴´�.
    // 2. �Է����κ��� ������ �����Ѵ�.
    // 3. ���⿡ ���� �÷��̾ �̵��Ѵ�.

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
        if (_bClimb) // �� Ÿ�� ���̸�
        {
            dir = new Vector3(h, v, 0).normalized;
            dir = Camera.main.transform.TransformDirection(dir);
            dir.z = 0.0f;
            _characterController.Move(dir * MoveSpeed * Time.deltaTime);
            CurStamina -= _staminaUseSpeed * Time.deltaTime;

            _yVelocity = 0.0f; // �߷� �ʱ�ȭ
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

        // 3. ���� ����
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

