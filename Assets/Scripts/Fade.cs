using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour
{
    private Material wallMaterial;
    private Color originalColor;
    private Coroutine currentFade;

    [Range(0f, 1f)]
    public float targetTransparentAlpha = 0f;

    public float fadeDuration = 1f;

    void Start()
    {
        wallMaterial = GetComponent<Renderer>().material; // Instantiates a unique material instance
        originalColor = wallMaterial.color;

        // Make sure alpha starts at full (opaque)
        Color startColor = wallMaterial.color;
        startColor.a = 1f;
        wallMaterial.color = startColor;
    }

    public void FadeOut()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeWall(wallMaterial.color.a, targetTransparentAlpha));
    }

    public void FadeIn()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeWall(wallMaterial.color.a, 1f));
    }

    private IEnumerator FadeWall(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0f;
        Color color = wallMaterial.color;

        while (timeElapsed < fadeDuration)
        {
            float t = timeElapsed / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            wallMaterial.color = color;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        wallMaterial.color = color;
    }
}