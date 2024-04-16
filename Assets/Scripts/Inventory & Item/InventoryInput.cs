using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject characterPanelGameObject;
    [SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] KeyCode[] toggleCharacterPanelKeys;
    [SerializeField] KeyCode[] toggleInventoryKeys;

    private bool _isOpen;
    public bool IsOpen {get {return _isOpen;}}


    void Update()
    {
        for(int i = 0; i < toggleCharacterPanelKeys.Length; i++)
        {
            if(Input.GetKeyDown(toggleCharacterPanelKeys[i]))
            {
                characterPanelGameObject.SetActive(!characterPanelGameObject.activeSelf);

                if(characterPanelGameObject.activeSelf)
                {
                    equipmentPanelGameObject.SetActive(true);
                    _isOpen = true;
                    ShowMouseCursor();
                }
                else
                {
                    _isOpen = false;
                    HideMouseCursor();
                }

                break;
            }
        }

        for(int i = 0; i < toggleInventoryKeys.Length; i++)
        {
            if(Input.GetKeyDown(toggleInventoryKeys[i]))
            {   
                if(!characterPanelGameObject.activeSelf)
                {
                    characterPanelGameObject.SetActive(true);
                    equipmentPanelGameObject.SetActive(false);
                    _isOpen = true;
                    ShowMouseCursor();
                }
                else if(equipmentPanelGameObject.activeSelf)
                {
                    equipmentPanelGameObject.SetActive(false);
                }
                else
                {
                    _isOpen = false;
                    characterPanelGameObject.SetActive(false);
                    HideMouseCursor();
                }
                break;
            }
        }
    }

    public void ShowMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleEquipmentPanel()
    {
        equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
    }
}
