using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionItem : MonoBehaviour
{
    public event Action OnYesEvent;
    public event Action OnNoEvent;

    public void Show()
    {
        gameObject.SetActive(true);
        OnYesEvent = null;
        OnNoEvent = null;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnYesButtonClick()
    {
        if (OnYesEvent != null)
            OnYesEvent();

        Hide();
    }

    public void OnNoButtonClick()
    {
        if (OnNoEvent != null)
            OnNoEvent();

        Hide();
    }
}
