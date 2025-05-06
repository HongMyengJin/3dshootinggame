using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private LayerMask targetLayer;

    private bool isHitboxActive = false;

    public void Activate() => isHitboxActive = true;
    public void Deactivate() => isHitboxActive = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ÇÃ·¹ÀÌ¾î Ä® Hit: " + other.name);
        if ((!isHitboxActive))
            return;
        if ((!LayerMaskExtensions.Contains(targetLayer, other.gameObject)))
            return;
        // if ((!isHitboxActive) || (!LayerMaskExtensions.Contains(targetLayer, other.gameObject))) return;

        Vector3 dir = (other.transform.position - transform.position).normalized;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            Damage damage = new Damage
            {
                Value = _damage,
                From = this.gameObject,
                Dir = dir
            };
            damageable.TakeDamage(damage);
        }
    }

}