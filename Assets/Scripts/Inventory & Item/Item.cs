using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID {get { return id; }}
    public string ItemName;
    public int DropChance;
    public Sprite Icon;
    [Range(1,999)]
    public int MaximumStacks = 1;

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    public virtual Item GetCopy()
    {
        return this;   
    }

    public virtual void Destroy()
    {
        
    }
}