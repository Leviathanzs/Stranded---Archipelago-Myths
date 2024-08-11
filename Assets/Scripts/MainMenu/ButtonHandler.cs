using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
   [SerializeField] Button startButton;

   public void StartButton()
   {
        SceneManager.LoadScene("Loading");
   }
}
