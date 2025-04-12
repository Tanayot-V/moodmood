using System;
using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public static Fader Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CanvasGroup mGroup;

    public static void Fade(bool isFadeIn, float fadeTime, Action onComplete = null)
    {
        Instance.StartCoroutine(FadeRoutine(Instance.mGroup, isFadeIn, fadeTime, onComplete));
    }
    public static void Fade(float fadeTime, Action OnSwitch, Action onComplete = null)
    {
        Instance.StartCoroutine(FadeRoutine(fadeTime, OnSwitch, onComplete));
    }

    public static IEnumerator FadeRoutine(float fadeTime, Action onSwitch, Action onComplete)
    {
        yield return FadeRoutine(Instance.mGroup, false, fadeTime * 0.5f, null);
        onSwitch?.Invoke();
        yield return FadeRoutine(Instance.mGroup, true, fadeTime * 0.5f, onComplete);
    }

    private static IEnumerator FadeRoutine(CanvasGroup canvasGroup, bool isFadeIn, float fadeTime, Action onComplete)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = isFadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke();
    }
}
