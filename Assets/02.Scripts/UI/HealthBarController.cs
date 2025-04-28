using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2f, 0);

    private Transform _target;
    private Camera _mainCamera;

    public void Setup(Transform target, HealthComponent health)
    {
        _target = target;
        _mainCamera = Camera.main;

        health.OnHealthChanged += UpdateHealth;
    }

    private void UpdateHealth(float current, float max)
    {
        _healthSlider.value = current / max;
    }

    public void LookAtTarget()
    {
        if (_target == null || _mainCamera == null)
            return;

        transform.position = _target.position + _offset;
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }

    private void LateUpdate()
    {
        LookAtTarget();
    }
}
