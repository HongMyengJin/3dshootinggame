using UnityEditor.PackageManager;
using UnityEngine;

public class ConeAttackHandler : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;        // ���� �ִ� �Ÿ�
    [SerializeField] private float attackAngle = 60.0f;       // ��ä�� �ݰ� (60�� -> �¿� 120��)
    [SerializeField] private int attackDamage = 10;           // ������

    [Header("Layer Settings")]
    [SerializeField] private LayerMask targetLayer;           // ������ ��� ���̾�

    public void Attack()
    {
        // �ֺ� ��� �ݶ��̴� �˻� (SphereOverlap)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            // ���� üũ
            Vector3 toTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toTarget);

            if (angle <= attackAngle * 0.5f)
            {
                Debug.Log($"Hit {hitCollider.name}");

                if (hitCollider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {

                    Damage damage = new Damage
                    {
                        Value = 10,
                        From = this.gameObject,
                        Dir = toTarget
                    };

                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
