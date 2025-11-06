using UnityEngine;

public class StageTimer : MonoBehaviour
{
    private float startTime;
    private bool running = false;

    public void StartTimer()
    {
        startTime = Time.time;
        running = true;
    }

    public float StopTimer()
    {
        if (!running) return 0f;
        running = false;
        return Time.time - startTime;
    }
}