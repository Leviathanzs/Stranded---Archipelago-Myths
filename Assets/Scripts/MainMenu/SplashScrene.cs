using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    public Image splashImage;
    public float fadeInDuration = 2.0f;
    public float displayDuration = 2.0f;
    public float fadeOutDuration = 2.0f;
    public string nextSceneName;

    void Start()
    {
        StartCoroutine(FadeInSplashScreen());
    }

    IEnumerator FadeInSplashScreen()
    {
        // Set initial alpha to 0
        Color color = splashImage.color;
        color.a = 0f;
        splashImage.color = color;

        // Fade in
        float timer = 0f;
        while (timer <= fadeInDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / fadeInDuration);
            splashImage.color = color;
            yield return null;
        }

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        timer = 0f;
        while (timer <= fadeOutDuration)
        {
            timer += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(timer / fadeOutDuration);
            splashImage.color = color;
            yield return null;
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}