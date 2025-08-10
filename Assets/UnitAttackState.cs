using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    
    public float stopAttackDistance = 1.2f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackStateMaterial();
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();
            
            // moverse hacia el enemigo
            agent.SetDestination(attackController.targetToAttack.position);
            var damageToInflict = attackController.unitDamage;
            
            //Atacar al enemigo
            attackController.targetToAttack.GetComponent<Filibustero>().ReceiveDamage(damageToInflict);
            
            // Si la unidad deberia seguir atacando
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackDistance || attackController.targetToAttack == null)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }
    
    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);
        
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
