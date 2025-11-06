using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}

