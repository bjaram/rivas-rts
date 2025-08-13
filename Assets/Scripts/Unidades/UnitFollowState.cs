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
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Verificación principal con variable temporal
        if(attackController.targetToAttack == null || 
           !attackController.targetsInRange.Contains(attackController.targetToAttack))
        {
            // Buscar nuevo objetivo
            attackController.SetNewTarget();
        }

        Transform currentTarget = attackController.targetToAttack;
    
        if (currentTarget == null)
        {
            animator.SetBool("isFollowing", false);
            return;
        }

        // Movimiento hacia el objetivo
        if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            agent.SetDestination(currentTarget.position);
            animator.transform.LookAt(currentTarget); // Corregido: antes miraba a sí mismo
        }

        // VERIFICACIÓN ADICIONAL DE NULL ANTES DE CALCULAR DISTANCIA
        if (currentTarget != null)
        {
            float distanceFromTarget = Vector3.Distance(
                currentTarget.position, 
                animator.transform.position
            );

            if (distanceFromTarget < attackingDistance)
            {
                animator.SetBool("isAttacking", true);
            }
        }
    }
}
