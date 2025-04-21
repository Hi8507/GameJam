using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMove : MonoBehaviour
{
    public GameObject Cube;

    [SerializeField]
    private Camera Camera = null;
    private NavMeshAgent Agent;

    private RaycastHit[] Hits = new RaycastHit[1];

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    private bool isWalking = false;

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    private bool isGrounded = false;
    public bool Jumped = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            
            //Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            //if (Physics.RaycastNonAlloc(ray, Hits) > 0)
            //{
            //    Agent.SetDestination(Hits[0].point);
            //}
        }

        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
        CheckGround();
    }
    private void FixedUpdate()
    {
        if (!playerCanMove) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<Animator>().SetBool("sneak", true);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("sneak", false);
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<BoxCollider>().enabled = false;
        }

        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDir.magnitude > 0)
        {
            // Get camera-forward and camera-right directions (flattened on Y)
            Vector3 camForward = Camera.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = Camera.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            // Combine input with camera directions
            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
            moveDir.Normalize();

            // Move the Rigidbody
            Vector3 targetVelocity = moveDir * walkSpeed;
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // Rotate the Cube to face movement direction
            Cube.transform.rotation = Quaternion.LookRotation(moveDir);

            GetComponent<Animator>().SetBool("Walk", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walk", false);
        }

        CheckGround();
    }


    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            GetComponent<Animator>().SetTrigger("jump");
            Jumped = true;
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;


        }

    }

}