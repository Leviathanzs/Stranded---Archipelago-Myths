using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootNotificationItem : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();

        if (backgroundImage != null)
        {
            backgroundImage.enabled = false; // nonaktifkan saat spawn
        }
    }

    public void Setup(Item item)
    {
        itemNameText.text = item.ItemName;

        if (item.Icon != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = item.Icon;
        }
        else
        {
            iconImage.enabled = false;
        }

        if (backgroundImage != null)
        {
            backgroundImage.enabled = true; // aktifkan background
        }

        Destroy(gameObject, 2f);
    }

}
