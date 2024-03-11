using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    [SerializeField] string floatName;
    [SerializeField] float valueOnEnter, valueOnExit;
    [SerializeField] bool updateOnStateEnter, updateOnStateExit;
    [SerializeField] bool updateOnStateMachineEnter, updateOnStateMachineExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnStateEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnStateExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }

   override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachineEnter)
        animator.SetFloat(floatName, valueOnEnter);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachineExit)
        animator.SetFloat(floatName, valueOnExit);
    }
}
