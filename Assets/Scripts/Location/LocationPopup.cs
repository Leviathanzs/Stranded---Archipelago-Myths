using System.Collections;
using UnityEngine;
using TMPro;

public class LocationPopup : MonoBehaviour
{
    public TextMeshProUGUI locationText;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f;
    public float showDuration = 2f;

    private void Start()
    {
        ShowLocation("The Spirit Hollow");
    }

    public void ShowLocation(string locationName)
    {
        locationText.text = locationName;
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        canvasGroup.alpha = 0f;

        // Fade in
        float time = 0f;
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(showDuration);

        // Fade out
        time = 0f;
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
