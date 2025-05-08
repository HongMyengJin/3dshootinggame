using UnityEditor.PackageManager;
using UnityEngine;

public class ConeAttackHandler : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;        // 공격 최대 거리
    [SerializeField] private float attackAngle = 60.0f;       // 부채꼴 반각 (60도 -> 좌우 120도)
    [SerializeField] private int attackDamage = 10;           // 데미지

    [Header("Layer Settings")]
    [SerializeField] private LayerMask targetLayer;           // 공격할 대상 레이어

    public void Attack()
    {
        // 주변 모든 콜라이더 검색 (SphereOverlap)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            // 각도 체크
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
