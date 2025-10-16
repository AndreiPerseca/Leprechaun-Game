using System.Collections;
using UnityEngine;
using TMPro;

public class IntroTextController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float displayTime = 4f;

    void Start()
    {
        // Start fade in
        StartCoroutine(ShowIntro());
    }

    IEnumerator ShowIntro()
    {
        // Fade in
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        // Optional: disable object
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
