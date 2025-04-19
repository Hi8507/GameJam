using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

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
        if (playerCanMove)
        {

            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") , 0, Input.GetAxis("Vertical"));

            //if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            //{
            //    isWalking = true;
            //}
            //else
            //{
            //    isWalking = false;
            //}
            if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("Walk", true);
                if (Input.GetKey(KeyCode.W))
                    Cube.transform.rotation= Quaternion.Euler(0, 90, 0);

                if (Input.GetKey(KeyCode.A))
                    Cube.transform.rotation = Quaternion.Euler(0, 0, 0);

                if (Input.GetKey(KeyCode.S))
                    Cube.transform.rotation = Quaternion.Euler(0, -90, 0);

                if (Input.GetKey(KeyCode.D))
                    Cube.transform.rotation = Quaternion.Euler(0, 180, 0);


            }

            else {
               GetComponent<Animator>().SetBool("Walk", false);
            }

        targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);


        }



        // All movement calculations while walking

        
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