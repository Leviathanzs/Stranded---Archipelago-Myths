using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;

    private void OnValidate()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        NameText = texts[0];
        ValueText = texts[1];
    }
}
