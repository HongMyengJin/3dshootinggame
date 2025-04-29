using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

public class Bomb : MonoBehaviour
{
    // 목표: 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    public GameObject ExplosionEffectPrefab;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        OffSimulation();
    }

    // 충돌했을 때

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"충돌~~:{collision.gameObject.name}");
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        gameObject.SetActive(false);
    }

    public void OffSimulation()
    {
        _rigidbody.useGravity = false;
        _collider.enabled = false;
        //_rigidbody.isKinematic = true;
        //_rigidbody.linearVelocity = Vector3.zero;  // 현재 속도도 0으로 초기화
        //_rigidbody.angularVelocity = Vector3.zero;  // 회전 속도도 0으로 초기화
    }

    public void OnSimulation()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _collider.enabled = true;
    }

    public void AddForce(Vector3 dir, float Power)
    {
        OnSimulation();
        _rigidbody.AddForce(dir * Power, ForceMode.Impulse);
    }
}
