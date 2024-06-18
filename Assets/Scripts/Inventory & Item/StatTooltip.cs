using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StatNameText;
    [SerializeField] TextMeshProUGUI StatModifiersLabelText;
    [SerializeField] TextMeshProUGUI StatModifiersText;

    private StringBuilder sb = new StringBuilder();
    public void ShowTooltip(PlayerBaseStats stat, string statName)
    {
        StatNameText.text = GetStatTopText(stat, statName);
        StatModifiersText.text = GetStatModifiersText(stat); 

        gameObject.SetActive(true);
    }   

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private string GetStatTopText(PlayerBaseStats stat, string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if(stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);

            if(stat.Value > stat.BaseValue)
                sb.Append("+");

            sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");
        }
        return sb.ToString();
    }

    private string GetStatModifiersText(PlayerBaseStats stat)
    {
        sb.Length = 0;

        foreach(StatModifier mod in stat.StatModifiers)
        {
            if(sb.Length > 0)
                sb.AppendLine();

            if(mod.Value > 0)
                sb.Append("+");

            if(mod.Type == StatModType.Flat)
            {
                sb.Append(mod.Value);
            }
            else
            {
                sb.Append(mod.Value * 100);
                sb.Append("%");
            }

            Item item = mod.Source as Item;

            if(item != null)
            {
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.Log("Modifier is not an Item");
            }
        }
        return sb.ToString();
    }
}
