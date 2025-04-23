using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    private float _moveSpeed = 7f;
    private CharacterController _characterController;
    private int _jumpN = 0;
    private bool _isDash = false;
    private bool _isClimb = false;
    private Vector3 _dir = Vector3.zero;
    private float _yVelocity = 0f;

    public Slider Slider;
    public PlayerSO PlayerDataSo;

    public PlayerStat PlayerStat;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() 
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (Mathf.Abs(dir.x) > 0.0f || Mathf.Abs(dir.z) > 0.0f)
            _dir = dir;
        dir = Camera.main.transform.TransformDirection(dir);

        if (Climb(dir, h, v))
            return;

        Jump();
        Run();
        Dash();

        BasicStamina();
        dir = GetVelocity(dir);

        if (_isDash)
            return;
        _characterController.Move(dir * _moveSpeed * Time.deltaTime);
    }

    public Vector3 GetVelocity(Vector3 dir)
    {
        _yVelocity += PlayerDataSo.Gravity * Time.deltaTime;
        dir.y = _yVelocity;

        return dir;
    }


    IEnumerator CoDash(float dashTime)
    {
        float elapseTime = 0.0f;
        while (elapseTime < dashTime)
        {
            elapseTime += Time.deltaTime;
            _characterController.Move(transform.forward * _moveSpeed * 5.0f * Time.deltaTime);
            yield return null;
        }

        _isDash = false;
    }


    public bool Climb(Vector3 dir, float h, float v)
    {

        float climbValue = PlayerDataSo.StaminaUseSpeed * Time.deltaTime;
        if (CheckClimb() && Input.GetKey(KeyCode.K) && climbValue < PlayerStat.CurStamina) // �� Ÿ�� ���̸�
        {
            dir = new Vector3(h, v, 0).normalized;
            dir = Camera.main.transform.TransformDirection(dir);
            dir.z = 0.0f;
            _characterController.Move(dir * _moveSpeed * Time.deltaTime);
            PlayerStat.CurStamina -= climbValue;
            PlayerStat.ChangeStamina();
            _yVelocity = 0.0f; // �߷� �ʱ�ȭ
            Debug.Log($"Stat: {PlayerStat.CurStamina} ��� �� ���¹̳� {climbValue}");
            Debug.Log("Ŭ���̹� ��!");
            return true;
        }
        Debug.Log("Ŭ���̹� ����!");
        return false;
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _jumpN = 0;
        }

        // 3. ���� ����
        if (Input.GetButtonDown("Jump") && _jumpN < 2)
        {
            _yVelocity = PlayerDataSo.JumpPower;
            _jumpN++;
        }
    }

    public void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // ���¹̳� 0���� ������ üũ
            _moveSpeed = 12.0f;

            PlayerStat.CurStamina -= Time.deltaTime * PlayerDataSo.StaminaUseSpeed;
            PlayerStat.ChangeStamina();
        }
    }

    public void Dash()
    {
        if (_isDash == false && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerStat.CurStamina - PlayerDataSo.StaminaDashUseSpeed < 0)
                return;

            PlayerStat.CurStamina -= PlayerDataSo.StaminaDashUseSpeed;
            PlayerStat.ChangeStamina();
            _isDash = true;
            _dir = Camera.main.transform.TransformDirection(_dir);
            StartCoroutine(CoDash(0.5f));
        }
    }

    public void BasicStamina()
    {
        if (!_isDash)
        {
            _moveSpeed = 7.0f;

            if (PlayerStat.CurStamina < PlayerDataSo.MaxStamina)
            {
                PlayerStat.CurStamina += Time.deltaTime * PlayerDataSo.StaminaFillSpeed;
                Debug.Log("���� ���¹̳� ���� ��!");
            }
                
            PlayerStat.ChangeStamina();
        }
    }

    public bool CheckClimb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f)){
            if (hit.collider.CompareTag("Wall"))
                return true;
        }

        return false;
    }
}

