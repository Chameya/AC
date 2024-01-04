using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;
    public string isUsingRootMotionBool;
    public bool isUsingRootMotionStatus;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool,isInteractingStatus);
        animator.SetBool(isInteractingBool,isUsingRootMotionStatus);
        animator.SetBool("RightPunch", false); 
        animator.SetBool("LeftPunch", false);
    }

}
