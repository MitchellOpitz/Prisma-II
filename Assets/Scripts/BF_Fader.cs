using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BF_Fader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    // Public getter for fadeDuration
    public float FadeDuration
    {
        get { return fadeDuration; }
    }

    private void Start()
    {
        InitializeFader();
    }

    private void InitializeFader()
    {
        if (fadeImage != null)
        {
            SetFadeActive(false);
            FadeIn();
        }
        else
        {
            Debug.LogError("FadeImage reference is not set.");
        }
    }

    public void FadeIn()
    {
        SetFadeActive(true);
        StartCoroutine(FadeToAlpha(0.0f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeToAlpha(1.0f));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }

    private void SetFadeActive(bool active)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(active);
        }
    }
}
