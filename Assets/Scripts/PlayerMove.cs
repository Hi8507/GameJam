using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    public float runSpeed = 12f;
    public float maxVelocityChange = 10f;

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    private bool isGrounded = false;
    public bool Jumped = false;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        animator.SetBool("jump", !isGrounded); // Update jump status
        CheckGround();
    }

    private void FixedUpdate()
    {
        if (!playerCanMove) return;

        // Sneaking
        bool isSneaking = Input.GetKey(KeyCode.LeftControl);
        animator.SetBool("sneak", isSneaking);

        GetComponent<CapsuleCollider>().enabled = !isSneaking;
        GetComponent<BoxCollider>().enabled = isSneaking;

        if (isSneaking && animator.GetFloat("Speed") < 1 && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("crouch walk")))
        { 
            animator.CrossFade("crouch", 0.1f);
        }

        // Movement
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDir.magnitude > 0)
        {
            // Movement direction relative to camera
            Vector3 camForward = Camera.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = Camera.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
            moveDir.Normalize();

            // Walk or run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float speed = isRunning ? runSpeed : walkSpeed;

            // Set the Speed parameter in the animator
            animator.SetFloat("Speed", speed); // Use float parameter for smooth transition

            // Movement
            Vector3 targetVelocity = moveDir * speed;
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            Cube.transform.rotation = Quaternion.LookRotation(moveDir);
        }
        else
        {
            // Stop movement and reset animations
            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0, 0.2f);
            velocity.z = Mathf.Lerp(velocity.z, 0, 0.2f);
            rb.velocity = velocity;

            animator.SetFloat("Speed", 0); // Set Speed to 0 when not moving
            animator.SetBool("sneak", false);
        }

        CheckGround();
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = Vector3.down;
        float distance = .75f;

        isGrounded = Physics.Raycast(origin, direction, out _, distance);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            StartCoroutine(JumpAfterAnimation());
        }
    }

    private IEnumerator JumpAfterAnimation()
    {
        animator.SetTrigger("jump");
        Jumped = true;

        yield return new WaitForSeconds(0.3f);

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("moving"))
        {
            transform.parent = collision.transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("moving"))
        {
            transform.parent = null;
        }
    }

    // Unused

    public void StartPushPull()
    {
        animator.SetTrigger("PushPullStart");
        animator.SetBool("PushPull", true);
    }

    public void StopPushPull()
    {
        animator.SetTrigger("PushPullStop");
        animator.SetBool("PushPull", false);
    }

    public void HangJump()
    {
        animator.SetTrigger("HangJump");
    }

    public void SetClimbing(bool isClimbing)
    {
        animator.SetBool("Climb", isClimbing);
    }

    public void SetHanging(bool isHanging)
    {
        animator.SetBool("Hanging", isHanging);
    }
}
