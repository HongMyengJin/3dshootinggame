using UnityEngine;

public class SwordWeaponStrategy : IWeaponStrategy
{
    private Animator animator;

    public SwordWeaponStrategy(Animator animator)
    {
        this.animator = animator;
    }

    public void Attack()
    {
        Debug.Log("∞À »÷µŒ∏£±‚!");
        animator.SetTrigger("Swing");
    }
}