using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;

public class DissolveController : MonoBehaviour
{
    public Material BaseMaterial;
    public float FadeDuration = 0.5f;
    public UnityEvent OnShieldShown;
    public UnityEvent OnShieldHidden;

    public UnityEvent OnStartEvent;

    private Material _instanceMaterial;
    private Renderer _renderer;
    private Coroutine _fadeRoutine;

    void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        InitializeMaterial();
        SetDissolve(1.0f);

        OnStartEvent?.Invoke();
    }

    private void InitializeMaterial()
    {
        _instanceMaterial = Instantiate(BaseMaterial);
        _renderer.material = _instanceMaterial;
    }

    public void Hide()
    {
        StartDissolve(1f, OnShieldShown);
    }

    public void Show()
    {
        StartDissolve(0f, OnShieldHidden);
    }

    private void StartDissolve(float targetAlpha, UnityEvent callback = null)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, callback));
    }

    private IEnumerator FadeRoutine(float targetdissolve, UnityEvent callback)
    {
        float startDissovle = _instanceMaterial.GetFloat("_Dissolve");
        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / FadeDuration;
            float dissolve = Mathf.Lerp(startDissovle, targetdissolve, t);
            SetDissolve(dissolve);
            yield return null;
        }

        SetDissolve(targetdissolve);
        callback?.Invoke();
    }

    private void SetDissolve(float dissolve)
    {
        if (_instanceMaterial == null)
            return;
        _instanceMaterial.SetFloat("_Dissolve", dissolve);
    }
}