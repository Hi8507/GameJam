using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBehavior : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float detectionRange = 10f;
    public float loseSightTime = 3f;
    public Transform player;
    public LayerMask visionMask;
    public float viewAngle = 60f;

    private NavMeshAgent agent;
    private Animator anim;
    private int currentPatrolIndex = 0;

    private enum AIState { Patrol, Chase }
    private AIState currentState;

    private float timeSinceLastSeen = 0f;

    // Speed values
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = AIState.Patrol;
        agent.speed = walkSpeed; // Set initial speed to walking speed
        GoToNextPatrolPoint();
        SetState("walk");
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                if (CanSeePlayer())
                {
                    SetState("run");
                    currentState = AIState.Chase;
                    agent.speed = runSpeed; // Increase speed when chasing
                }
                break;

            case AIState.Chase:
                agent.SetDestination(player.position);

                if (CanSeePlayer())
                {
                    timeSinceLastSeen = 0f;
                }
                else
                {
                    timeSinceLastSeen += Time.deltaTime;
                    if (timeSinceLastSeen >= loseSightTime)
                    {
                        currentState = AIState.Patrol;
                        SetState("walk");
                        agent.speed = walkSpeed; // Reset speed back to walking when patrolling
                        GoToNextPatrolPoint();
                    }
                }
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < viewAngle / 2f)
            {
                if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, detectionRange, visionMask))
                {
                    if (hit.transform == player)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void SetState(string newState)
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);

        switch (newState)
        {
            case "walk":
                anim.SetBool("isWalking", true);
                break;
            case "run":
                anim.SetBool("isRunning", true);
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Vector3 forward = transform.forward;
            Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
            Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

            Gizmos.DrawLine(transform.position, transform.position + left * detectionRange);
            Gizmos.DrawLine(transform.position, transform.position + right * detectionRange);
            Gizmos.DrawLine(transform.position, transform.position + forward * detectionRange);

            DrawCone(transform.position, forward, detectionRange, viewAngle);

            if (CanSeePlayer())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
    }

    void DrawCone(Vector3 position, Vector3 forward, float range, float angle)
    {
        float step = angle / 10;
        for (float i = -angle / 2; i < angle / 2; i += step)
        {
            Vector3 direction = Quaternion.Euler(0, i, 0) * forward;
            Gizmos.DrawLine(position, position + direction * range);
        }
    }
}
