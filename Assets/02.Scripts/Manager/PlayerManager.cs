using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    PlayerWeaponHandler _playerWeaponHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerWeaponHandler = FindAnyObjectByType<PlayerWeaponHandler>();
        // _player = FindAnyObjectByType<PlayerMove>();
    }
    
    public bool IsAttack()
    {
        return _playerWeaponHandler.IsAttack();
    }

    public void EnableControl()
    {
        // _player.EnableControl();
    }

    public void DisableControl()
    {
        // _player.DisableControl();
    }
}