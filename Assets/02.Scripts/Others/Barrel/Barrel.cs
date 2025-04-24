using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using static Enemy;

public class Barrel : MonoBehaviour
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
            StartCoroutine(DelayedDestroy(DestroyTime));
            return;
        }

        EffectManager.Instance.Play(EffectType.BarrelHit, transform.position);
    }

    public void TriggerExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);

        foreach(Collider collider in colliders)
        {
            GameObject gameObject = collider.gameObject;
            Enemy barrel = gameObject.GetComponent<Enemy>();
            if (gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = gameObject.GetComponent<Enemy>();
                Damage damage = new Damage
                {
                    Value = 10,
                    From = this.gameObject
                };
                enemy.TakeDamage(damage, Vector3.zero);
            }
            else if(gameObject.CompareTag("Player"))
            {
                PlayerMove playerMove = gameObject.GetComponent<PlayerMove>();
                Damage damage = new Damage
                {
                    Value = 10,
                    From = this.gameObject
                };

                Debug.Log("Player 맞음!");
                //
            }
        }
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

    IEnumerator DelayedDestroy(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
