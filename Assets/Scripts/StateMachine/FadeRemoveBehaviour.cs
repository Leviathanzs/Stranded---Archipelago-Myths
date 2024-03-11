using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    [SerializeField] private float fadeTime = .5f;
    float timeElapsed = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        if(timeElapsed > fadeTime)
        {
            Destroy(objToRemove);
        }
    }

}
