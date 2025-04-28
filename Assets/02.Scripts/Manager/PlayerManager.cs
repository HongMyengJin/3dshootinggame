using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerMove _player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _player = FindAnyObjectByType<PlayerMove>();
    }
    
    public void EnableControl()
    {
        _player.EnableControl();
    }

    public void DisableControl()
    {
        _player.DisableControl();
    }
}