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

    // Pause configuration
    public float pauseDuration = 2f;  // How long the enemy will pause at each point
    private float pauseTimer = 0f;    // Timer to track remaining pause time
    private bool isPaused = false;    // Flag to track paused state

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
        // If the agent is paused, handle the pause timer
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;

            // Resume movement when the pause timer expires
            if (pauseTimer <= 0)
            {
                isPaused = false;
                agent.isStopped = false;
                // Set new destination after the pause
                currentTarget = (currentTarget == pointA) ? pointB : pointA;
                agent.SetDestination(currentTarget.position);
            }
            return; // Skip the rest of the patrol logic while paused
        }

        // When not paused, check if we've reached the destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Start the pause
            pauseTimer = pauseDuration;
            isPaused = true;
            agent.isStopped = true;
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach (Collider hit in hits)
        {
            Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);
            // Raycast to check line of sight
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