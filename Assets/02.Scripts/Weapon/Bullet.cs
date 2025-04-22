using UnityEditor.PackageManager;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 목표: 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    public ParticleSystem BulletEffect;

    private void Awake()
    {
        BulletEffect = Instantiate(BulletEffect);
        BulletEffect.transform.parent = gameObject.transform;
    }
    public void PlayEffect(Vector3 position)
    {
        BulletEffect.gameObject.SetActive(true);
        BulletEffect.transform.position = position;
        BulletEffect.Play();
    }

}
