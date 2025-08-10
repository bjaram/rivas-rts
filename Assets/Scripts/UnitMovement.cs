using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask groundLayer;
    
    public bool isCommandedToMove = false;
    
    void Start()
    {
      cam = Camera.main;
      agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                isCommandedToMove = true;
                agent.SetDestination(hit.point);
            }
        }

        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
    }
}
