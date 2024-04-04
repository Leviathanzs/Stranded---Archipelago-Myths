using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemNameText;
    [SerializeField] TextMeshProUGUI ItemSlotText;
    [SerializeField] TextMeshProUGUI ItemStatsText;

    private StringBuilder sb = new StringBuilder();
    public void ShowTooltip(EquippableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.EquipmentType.ToString();

        sb.Length = 0;
        AddStat(item.StrenghtBonus, "Strenght");
        AddStat(item.AgilityBonus, "Agility");
        AddStat(item.IntelligenceBonus, "Intelligence");
        AddStat(item.VitalityBonus, "Vitality");

        AddStat(item.StrenghtPercentBonus, "Strenght", isPercent : true);
        AddStat(item.AgilityPercentBonus, "Agility", isPercent : true);
        AddStat(item.IntelligencePercentBonus, "Intelligence", isPercent : true);
        AddStat(item.VitalityPercentBonus, "Vitality", isPercent : true);

        ItemStatsText.text = sb.ToString();

        gameObject.SetActive(true);
    }   

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if(value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();

            if(value > 0)
                sb.Append("+");

            if(isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }
}
