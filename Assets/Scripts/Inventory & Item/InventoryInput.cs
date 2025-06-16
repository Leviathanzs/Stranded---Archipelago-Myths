using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject characterPanelGameObject;
    [SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] KeyCode[] toggleCharacterPanelKeys;
    [SerializeField] KeyCode[] toggleInventoryKeys;
    [SerializeField] AudioSource inventorySfx;

    private bool _isOpen;
    public bool IsOpen { get { return _isOpen; } }

    // Variabel untuk menyimpan skala asli panel karakter
    private Vector3 _originalScale;

    void Start()
    {
        // Simpan skala asli sebelum menyembunyikan panel
        _originalScale = characterPanelGameObject.transform.localScale;
        HideCharacterPanel(); // mulai dalam keadaan tersembunyi
        _isOpen = false;
        HideMouseCursor();
    }

    void ShowCharacterPanel()
    {
        characterPanelGameObject.transform.localScale = _originalScale;
        inventorySfx.Play();
    }

    void HideCharacterPanel()
    {
        characterPanelGameObject.transform.localScale = Vector3.zero;
    }

    bool IsCharacterPanelVisible()
    {
        return characterPanelGameObject.transform.localScale != Vector3.zero;
    }

    void Update()
    {
        foreach (KeyCode key in toggleCharacterPanelKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (!IsCharacterPanelVisible())
                {
                    ShowCharacterPanel();
                    equipmentPanelGameObject.SetActive(true);
                    _isOpen = true;
                    ShowMouseCursor();
                }
                else
                {
                    HideCharacterPanel();
                    _isOpen = false;
                    HideMouseCursor();
                }
                break;
            }
        }

        foreach (KeyCode key in toggleInventoryKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (!IsCharacterPanelVisible())
                {
                    ShowCharacterPanel();
                    equipmentPanelGameObject.SetActive(false);
                    _isOpen = true;
                    ShowMouseCursor();
                }
                else if (equipmentPanelGameObject.activeSelf)
                {
                    equipmentPanelGameObject.SetActive(false);
                }
                else
                {
                    HideCharacterPanel();
                    _isOpen = false;
                    HideMouseCursor();
                }
                break;
            }
        }
    }

    public void ToggleEquipmentPanel()
    {
        equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
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
}
