using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    [SerializeField] string boolName;
    [SerializeField] bool valueOnEnter, valueOnExit;
    [SerializeField] bool updateOnState;
    [SerializeField] bool updateOnStateMachine;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachine)
        animator.SetBool(boolName, valueOnEnter);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachine)
        animator.SetBool(boolName, valueOnExit);
    }
}
