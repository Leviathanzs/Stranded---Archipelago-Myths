using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallaxBackground : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    private Vector2 startingPosition;
    private float startingZ;

    private Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    private float zDistanceFromTarget => followTarget != null ? transform.position.z - followTarget.position.z : 0f;

    private float clippingPlane => cam != null ? cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane) : 100f;

    private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (followTarget == null)
            TryAssignFollowTarget();

        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryAssignFollowTarget();
    }

    void Update()
    {
        if (cam == null || followTarget == null)
            return;

        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }

    private void TryAssignFollowTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            followTarget = player.transform;
        }
    }
}
