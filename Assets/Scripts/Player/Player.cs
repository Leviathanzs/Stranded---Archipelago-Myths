using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Player : MonoBehaviour
{
    private static Player instance;
    public PlayerBaseStats Strenght;
    public PlayerBaseStats Agility;
    public PlayerBaseStats Intelligence;
    public PlayerBaseStats Vitality;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
        Debug.Log("Karakter instance disimpan");
    }
}