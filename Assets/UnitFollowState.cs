using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    public float attackingDistance = 1f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        attackController.SetFollowStateMaterial();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // La unidad deberia transicionar al Idle State si no hay objetivo
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {
            if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
            {
                agent.SetDestination(attackController.targetToAttack.position);
                animator.transform.LookAt(attackController.transform);
            }
        }
        
        //la unidad deberia atacar al objetivo si esta dentro de la distancia de ataque
        /*float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
        if (distanceFromTarget < attackingDistance)
        {
            agent.SetDestination(animator.transform.position);
            animator.SetBool("isAttacking", true);
        }*/
    }
}
