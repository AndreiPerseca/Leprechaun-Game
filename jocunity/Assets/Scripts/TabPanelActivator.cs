using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TabPanelActivatorWithFade : MonoBehaviour
{
    [Header("Settings")]
    public float activeDuration = 3f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;
    private Coroutine autoHideCoroutine;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // IMPORTANT: Panel-ul rămâne activ → doar alpha = 0
        // → Ca să NU intre în conflict cu IntroTextController
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        // Dacă fade out era în progres → îl opresc
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Pornim fade IN
        fadeCoroutine = StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Dacă auto-hide era activ → îl opresc
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
        }

        // Pornim timer de ascundere
        autoHideCoroutine = StartCoroutine(AutoHideAfterDelay());
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;

        // Setăm interactivitatea → activ când fade in, dezactiv la fade out
        if (endAlpha > 0f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        canvasGroup.alpha = startAlpha;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        fadeCoroutine = null;
    }

    private IEnumerator AutoHideAfterDelay()
    {
        yield return new WaitForSeconds(activeDuration);

        // Fade OUT după delay
        fadeCoroutine = StartCoroutine(Fade(1f, 0f, fadeDuration));
    }
}
