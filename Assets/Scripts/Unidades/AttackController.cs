using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    public List<Transform> targetsInRange = new List<Transform>(); // Lista de objetivos
    
    public int unitDamage;
    public bool isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(isPlayer && other.CompareTag("Enemigo"))
        {
            // Añadir a la lista de objetivos
            if(!targetsInRange.Contains(other.transform))
            {
                targetsInRange.Add(other.transform);
                Debug.Log("Objetivo agregado: " + other.name);
            }
            
            // Asignar objetivo si no hay uno actual
            if(targetToAttack == null)
            {
                SetNewTarget();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(isPlayer && other.CompareTag("Enemigo"))
        {
            // Remover de la lista
            if(targetsInRange.Contains(other.transform))
            {
                targetsInRange.Remove(other.transform);
                Debug.Log("Objetivo removido: " + other.name);
            }
            
            // Si el objetivo actual salió, buscar uno nuevo
            if(targetToAttack == other.transform)
            {
                SetNewTarget();
            }
        }
    }
    
    public void SetNewTarget()
    {
        if(targetsInRange.Count == 0)
        {
            targetToAttack = null;
            Debug.Log("Sin objetivos");
            return;
        }
        
        // Seleccionar el objetivo MÁS CERCANO
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        
        foreach(Transform target in targetsInRange)
        {
            if(target == null) continue; // Saltar si fue destruido
            
            float distance = Vector3.Distance(transform.position, target.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        
        targetToAttack = closestTarget;
        if(targetToAttack != null)
            Debug.Log("Nuevo objetivo: " + targetToAttack.name);
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
