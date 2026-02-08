using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonEntrance : CollidableObject
{
    public string dungeonSceneName;
    
    private bool isEntered = false;
    
    // Optional: Reference to StageController if needed to save state before leaving
    private StageController stageController;

    protected override void Start()
    {
        base.Start();
        stageController = FindObjectOfType<StageController>();
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        if (isEntered) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (collidedObject.CompareTag("Player"))
            {
                EnterDungeon();
            }
        }
    }

    private void EnterDungeon()
    {
        isEntered = true;

        // Save state or notify stage controller if necessary
        if (stageController != null)
        {
            stageController.OnStageEnd();
        }

        SceneManager.LoadScene(dungeonSceneName);
    }
}
