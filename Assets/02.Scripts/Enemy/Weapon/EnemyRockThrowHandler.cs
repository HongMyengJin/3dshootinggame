using UnityEngine;

public class EnemyRockThrowHandler : MonoBehaviour
{
    [SerializeField] private GameObject _prefabRock;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private float _throwSpeed;
    [SerializeField] private Transform _target;
    private GameObject _currentRock;
    // 공 굴리기 Event
    public void SpawnRockEvent()
    {
        _currentRock = Instantiate(_prefabRock, _spawnPosition.position, _spawnPosition.rotation, _spawnPosition);
    }

    public void ThrowRockEvent()
    {
        Rigidbody rigidbody = _currentRock.GetComponent<Rigidbody>();
        Vector3 direction = (_target.position - _spawnPosition.position).normalized;

        _currentRock.transform.parent = null;

        rigidbody.isKinematic = false;

        rigidbody.AddForce(direction * _throwSpeed, ForceMode.Impulse);
    }
}
