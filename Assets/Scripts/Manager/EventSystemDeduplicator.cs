using UnityEngine;
using UnityEngine.EventSystems;

public class AutoEventSystemCleanup : MonoBehaviour
{
    void Awake()
    {
        EventSystem[] allEventSystems = FindObjectsOfType<EventSystem>();

        if (allEventSystems.Length > 1)
        {
            Debug.LogWarning($"Terdeteksi {allEventSystems.Length} EventSystem. Menghapus duplikat...");
            for (int i = 1; i < allEventSystems.Length; i++)
            {
                Destroy(allEventSystems[i].gameObject);
            }
        }
    }
}
