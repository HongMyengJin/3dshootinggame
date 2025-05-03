using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class BombWeaponStrategy : IWeaponStrategy
{
    private readonly Animator _animator;
    private readonly GameObject _weapon;
    private readonly Transform _weaponPosition;
    private readonly float _power = 50.0f;
    private GameObject _bombObject;
    private bool _throw = false;
    private GameObject _player;

    public Transform Self { get; }

    public BombWeaponStrategy(Animator animator, Transform weaponPosition, GameObject player)
    {
        _weaponPosition = weaponPosition;
        this._animator = animator;
        _player = player;
    }

    public void OnAttackInput()
    {
        _animator.SetTrigger("Throw");
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {
        BombAttack();
    }


    public void BombAttack()
    {
        _bombObject = BombManager.Instance.UseBomb(_weaponPosition.position);
        Bomb bomb = _bombObject.GetComponentInChildren<Bomb>();
        bomb.transform.rotation = _player.transform.rotation;
        bomb.OnSimulation();
        bomb.AddForce(_player.transform.forward, _power);
    }
}
