using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    private AttackController attackController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ver si hay un filibustero cerca
        if (attackController.targetToAttack != null)
        {
            //aca nos movemos hacia el Follow State
            animator.SetBool("isFollowing", true);
        }
    }
}
