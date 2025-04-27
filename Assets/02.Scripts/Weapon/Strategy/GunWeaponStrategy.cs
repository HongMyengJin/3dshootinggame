using UnityEngine;

public class GunWeaponStrategy : IWeaponStrategy
{
    private Transform firePoint;

    public GunWeaponStrategy(Transform firePoint)
    {
        this.firePoint = firePoint;
    }

    public void Attack()
    {
        Debug.Log("รั น฿ป็!");
    }
}
