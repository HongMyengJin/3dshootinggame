using UnityEngine;

public class EnemyRockThrowHandler : MonoBehaviour
{
    [SerializeField] private GameObject _prefabRock;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private float _throwSpeed;
    private GameObject _currentRock;
    // 공 굴리기 Event
    public void SpawnRockEvent()
    {
        _currentRock = Instantiate(_prefabRock, _spawnPosition.position, _spawnPosition.rotation, _spawnPosition);
    }

    public void ThrowRockEvent()
    {
        Rigidbody rigidbody = _currentRock.GetComponent<Rigidbody>();
        _currentRock.transform.parent = null;

        rigidbody.isKinematic = false;

        rigidbody.AddForce(_spawnPosition.forward * _throwSpeed, ForceMode.Impulse);
    }
}
