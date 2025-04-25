using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using static Enemy;

public class Barrel : MonoBehaviour, IDamageable
{
    public int Health = 30;
    public float Radius = 20.0f;
    public float DestroyTime = 3.0f;
    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0)
        {
            EffectManager.Instance.Play(EffectType.BarrelBomb, transform.position);
            TriggerExplosion();
            ExplodeSelf();
            return;
        }

        EffectManager.Instance.Play(EffectType.BarrelHit, transform.position);
    }

    public void TriggerExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, ~LayerMask.NameToLayer("Barrel"));

        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage
                {
                    Value = 10,
                    From = this.gameObject
                };
                damageable.TakeDamage(damage);
            }
        }

        Collider[] barrels = Physics.OverlapSphere(transform.position, Radius, LayerMask.NameToLayer("Barrel"));

        foreach (Collider collider in barrels)
        {
            if (collider.TryGetComponent(out Barrel barrel))
            {
                barrel.ExplodeSelf();
            }
        }
        Destroy(gameObject, DestroyTime);
    }

    public void ExplodeSelf(float force = 12.0f, float Power = 5.0f)
    {
        if(!TryGetComponent<Rigidbody>(out Rigidbody rb))
            return;

        // 앞 + 위로 날리기

        Vector3 direction = (transform.forward + Vector3.up * 1.5f + Random.insideUnitSphere * 0.5f).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);

        // 빙글 회전
        Vector3 randomTorque = Random.onUnitSphere * Power; 
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        EffectManager.Instance.Play(EffectType.BarrelBomb, transform.position);
    }
}
