using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    TextMeshProUGUI text;
    RectTransform textTransform;
    Color startColor;

    [SerializeField] private Vector3 moveSpeed = new Vector3(0, 75, 0);
    private float timeToFade = 1f;
    private float timeElapsed = 0;
    
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        textTransform = GetComponent<RectTransform>();
        startColor = text.color; 
    }

    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;
        if(timeElapsed <  timeToFade)
        {
            float fadeAlpha = startColor.a * (1- (timeElapsed / timeToFade));
            text.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
