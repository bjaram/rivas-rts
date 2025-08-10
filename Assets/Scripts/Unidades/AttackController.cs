using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    public int unitDamage;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemigo") && targetToAttack == null)
        {
            targetToAttack = other.transform;
            Debug.Log("Objetivo asignado: " + targetToAttack.name);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemigo") && targetToAttack != null)
        {
            Debug.Log("Objetivo perdido: " + targetToAttack.name);
            targetToAttack = null;
        }
    }
    
    public void SetIdleStateMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }
    
    public void SetFollowStateMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }
    
    public void SetAttackStateMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }
    
    private void OnDrawGizmos()
    {
        // Follow Distance / Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f*0.2f);
        
        // Attack Distance / Area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
        
        // Stop Attack Distance / Area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}
