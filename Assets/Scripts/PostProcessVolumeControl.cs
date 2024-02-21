using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessVolumeControl : MonoBehaviour
{
    public Volume postProcessVolume;
    public float fadeDuration = 1f;
    public float vignetteIntesity = 1f;
    public Vignette vignette;

    private void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);

        //vignetteIntesity = vignette.intensity.value;

        FadeOutVignette();
    }
    public void FadeInVignette()
    {
        StartCoroutine(FadeVignette(0, vignetteIntesity, fadeDuration));
    }

    // Method to fade out the vignette effect
    public void FadeOutVignette()
    {
        StartCoroutine(FadeVignette(vignette.intensity.value, 0, fadeDuration));
    }

    // Coroutine for fading the vignette effect
    private IEnumerator FadeVignette(float startIntensity, float endIntensity, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, t);
            yield return null;
        }
    }
}
