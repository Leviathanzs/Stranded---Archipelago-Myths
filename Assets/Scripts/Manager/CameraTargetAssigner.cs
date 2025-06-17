using UnityEngine;
using Cinemachine;

public class CameraTargetAssigner : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;

    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        AssignFollowTarget();
    }

    void AssignFollowTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            vCam.Follow = player.transform;
            Debug.Log("Cinemachine camera now following player.");
        }
        else
        {
            Debug.LogWarning("Player not found to assign as camera target.");
        }
    }
}
