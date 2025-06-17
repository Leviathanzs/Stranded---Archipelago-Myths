using UnityEngine;

public class PersistentSystemObject : MonoBehaviour
{
    private static PersistentSystemObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
