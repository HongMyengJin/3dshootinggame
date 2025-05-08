using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;

public class ShieldController : MonoBehaviour
{
    public Material BaseMaterial;
    public float FadeDuration = 0.5f;
    public UnityEvent OnShieldShown;
    public UnityEvent OnShieldHidden;

    private Material _instanceMaterial;
    private Renderer _renderer;
    private Coroutine _fadeRoutine;
    private bool _isVisible = true;

    void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        InitializeMaterial();
        Hide();
    }

    private void InitializeMaterial()
    {
        _instanceMaterial = Instantiate(BaseMaterial);
        _renderer.material = _instanceMaterial;
    }

    public void Show()
    {
        if (_isVisible) 
            return;
        _isVisible = true;
        StartFade(1f, OnShieldShown);
        _renderer.enabled = _isVisible;
    }

    public void Hide()
    {
        if (!_isVisible) 
            return;
        _isVisible = false;
        StartFade(0f, OnShieldHidden);
        _renderer.enabled = _isVisible;
    }

    private void StartFade(float targetAlpha, UnityEvent callback = null)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, callback));
    }

    private IEnumerator FadeRoutine(float targetAlpha, UnityEvent callback)
    {
        float startAlpha = _instanceMaterial.color.a;
        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / FadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        callback?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        if (_instanceMaterial == null)
            return;

        Color baseColor = _instanceMaterial.GetColor("_BaseColor");
        baseColor.a = alpha;
        _instanceMaterial.SetColor("_BaseColor", baseColor);
    }
}