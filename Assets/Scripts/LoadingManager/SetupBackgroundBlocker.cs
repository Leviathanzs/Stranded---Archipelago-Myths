using UnityEngine;
using UnityEngine.UI;

public class SetupBackgroundBlocker : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject characterPanel;

    private GameObject blockerPanel;

    void Start()
    {
        CreateBackgroundBlocker();
    }

    void CreateBackgroundBlocker()
    {
        blockerPanel = new GameObject("BackgroundBlockerPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        blockerPanel.transform.SetParent(canvas.transform, false);

        RectTransform rt = blockerPanel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = blockerPanel.GetComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.5f); 
        img.raycastTarget = true;

        // Posisikan blocker di paling bawah
        blockerPanel.transform.SetAsFirstSibling();

        // Posisikan CharacterPanel di atas blocker
        characterPanel.transform.SetAsLastSibling();
    }
}
