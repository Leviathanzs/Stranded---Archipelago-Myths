using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogueSystem : MonoBehaviour
{
[SerializeField] GameObject dialoguePanel;
[SerializeField] TextMeshProUGUI dialogueText;
[SerializeField] string[] dialogueLines;
private int currentLine = 0;
private bool isPlayerInRange;

void Update()
    {
        // If the player presses the interaction key and is near the NPC
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                // Continue dialogue or close if finished
                DisplayNextLine();
            }
            else
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);   // Show the dialogue UI
        currentLine = 0;                 // Start at the first line of dialogue
        dialogueText.text = dialogueLines[currentLine];
    }

    void DisplayNextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);  // Hide the dialogue UI
        
    }

    // Trigger detection for when the player is near the NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            EndDialogue();  // Automatically end dialogue if the player walks away
        }
    }
}