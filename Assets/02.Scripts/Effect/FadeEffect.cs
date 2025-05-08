using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Graphic targetGraphic;
    [SerializeField] private float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (targetGraphic != null)
            SetAlpha(0f);
    }

    public void Play(float startAlpha = 1f)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut(startAlpha));
    }

    private IEnumerator FadeOut(float startAlpha)
    {
        SetAlpha(startAlpha);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (targetGraphic == null)
            return;

        var color = targetGraphic.color;
        color.a = alpha;
        targetGraphic.color = color;
    }
}
