using UnityEngine;
using UnityEngine.AI;

public class SnapToNavMesh : MonoBehaviour
{
    [SerializeField] float maxDistance = 3f;

    void Start()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning($"SnapToNavMesh: no encontré NavMesh cerca de {name}. Sube maxDistance o revisa el bake.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = NavMesh.SamplePosition(transform.position, out _, 0.2f, NavMesh.AllAreas)
            ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.15f);
    }
}
