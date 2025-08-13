using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    
    public float stopAttackDistance = 1.2f;
    private float attackRate = 1f; // 1 segundo entre ataques
    private float attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform currentTarget = attackController.targetToAttack;
    
        if(attackController.targetToAttack == null || 
           !attackController.targetsInRange.Contains(attackController.targetToAttack))
        {
            attackController.SetNewTarget();
            currentTarget = attackController.targetToAttack;
        
            if(currentTarget == null)
            {
                animator.SetBool("isAttacking", false);
                return;
            }
        }

        float distance = Vector3.Distance(
            currentTarget.position, 
            animator.transform.position
        );

        if (distance > stopAttackDistance)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        LookAtTarget(currentTarget);
        
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        var damageToInflict = attackController.unitDamage;
        attackController.targetToAttack.GetComponent<Unit>().TakeDamage(damageToInflict);
    }
    
    private void LookAtTarget(Transform target)
    {
        Vector3 direction = target.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);
    
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
