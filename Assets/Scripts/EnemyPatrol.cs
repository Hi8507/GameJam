using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyPatrolNavMesh : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;

    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = pointB;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        Patrol();
        DetectPlayer();
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            agent.SetDestination(currentTarget.position);
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);

            // Raycast to check if there's a clear line of sight
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit rayHit, distanceToPlayer))
            {
                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("Player in sight! You Lose!");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }


    // Optional: visualize detection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
