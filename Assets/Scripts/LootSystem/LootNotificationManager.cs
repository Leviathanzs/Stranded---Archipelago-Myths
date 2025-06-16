using UnityEngine;

public class LootNotificationManager : MonoBehaviour
{
    public static LootNotificationManager Instance;

    [SerializeField] private Transform notificationParent;
    [SerializeField] private GameObject notificationPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowNotification(Item item)
    {
        GameObject notifGO = Instantiate(notificationPrefab, notificationParent);
        LootNotificationItem notif = notifGO.GetComponent<LootNotificationItem>();
        notif.Setup(item);
    }
}
