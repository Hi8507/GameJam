using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeedleMover : MonoBehaviour
{
    public GameObject DeathPanel;
    public AudioSource Scream;
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;

    private int currentPatrolIndex = 0;
    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            targetPosition = patrolPoints[0].position;
            hasTarget = true;
        }
    }

    void Update()
    {
        if (hasTarget)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            //transform.forward = direction;
        }
        else
        {
            currentPatrolIndex++;

            if (currentPatrolIndex < patrolPoints.Length)
            {
                targetPosition = patrolPoints[currentPatrolIndex].position;
            }
            else
            {
                hasTarget = false; // Reached the final patrol point
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            DeathPanel.SetActive(true);
            Scream.Play();
            Invoke("DeathJump", 4);
        }
    }

    public void DeathJump()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
