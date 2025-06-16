using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootNotificationItem : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI itemNameText;

    private Image backgroundImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Vector2 startOffset = new Vector2(200f, 0f); 
    private float fadeDuration = 0.3f;
    private float displayDuration = 2f;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // Mulai transparan dan di posisi offset
        if (canvasGroup != null) canvasGroup.alpha = 0f;
        if (rectTransform != null) rectTransform.anchoredPosition += startOffset;

        if (backgroundImage != null)
        {
            backgroundImage.enabled = false;
        }
    }

    public void Setup(Item item)
    {
        itemNameText.text = item.ItemName;

        if (item.Icon != null)
        {
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }

        if (backgroundImage != null)
        {
            backgroundImage.enabled = true;
        }

        // Jalankan animasi muncul & hilang
        StartCoroutine(AnimateAndDestroy());
    }

    private System.Collections.IEnumerator AnimateAndDestroy()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        // Fade-out
        time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
