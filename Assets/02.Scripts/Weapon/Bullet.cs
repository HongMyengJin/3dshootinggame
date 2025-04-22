using UnityEditor.PackageManager;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ��ǥ: ���콺�� ������ ��ư�� ������ ī�޶� �ٶ󺸴� �������� ����ź�� ������ �ʹ�.
    // 1. ����ź ������Ʈ �����
    // 2. ������ ��ư �Է� �ޱ�
    // 3. �߻� ��ġ�� ����ź �����ϱ�
    // 4. ������ ����ź�� ī�޶� �������� �������� �� ���ϱ�

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
