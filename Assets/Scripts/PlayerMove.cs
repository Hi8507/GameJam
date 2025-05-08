using DG.Tweening;
using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public GameObject Cube;

    [SerializeField]
    private Camera Camera = null;

    [SerializeField] private float cameraRotationAngle = 0f;
    public float cameraSnapDuration = 0.5f;
    public float cameraDistance = 3f;
    public float cameraHeightOffset = 1.5f;

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
    private bool isPaused = false;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        CheckGround();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SnapCameraHorizontal();
        }
    }

    private void FixedUpdate()
    {
        if (!playerCanMove) return;

        bool isSneaking = Input.GetKey(KeyCode.LeftControl);
        animator.SetBool("sneak", isSneaking);

        GetComponent<CapsuleCollider>().enabled = !isSneaking;
        GetComponent<BoxCollider>().enabled = isSneaking;

        if (isSneaking && animator.GetFloat("Speed") < 1 &&
            (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") ||
             animator.GetCurrentAnimatorStateInfo(0).IsName("crouch walk")))
        {
            animator.CrossFade("crouch", 0.1f);
        }

        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDir.magnitude > 0)
        {
            Vector3 camForward = Camera.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = Camera.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
            moveDir.Normalize();

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float speed = isRunning ? runSpeed : walkSpeed;

            animator.SetFloat("Speed", speed);

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
            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0, 0.2f);
            velocity.z = Mathf.Lerp(velocity.z, 0, 0.2f);
            rb.velocity = velocity;

            animator.SetFloat("Speed", 0);
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

    public void SnapCameraHorizontal()
    {
        Vector3 targetDirection = Quaternion.Euler(0, cameraRotationAngle, 0) * Vector3.forward;
        Vector3 targetPosition = Cube.transform.position - targetDirection.normalized * cameraDistance;
        targetPosition.y = Cube.transform.position.y + cameraHeightOffset;
        Vector3 lookTarget = Cube.transform.position + Vector3.up * cameraHeightOffset;

        Sequence cameraSequence = DOTween.Sequence();
        cameraSequence.Join(Camera.transform.DOMove(targetPosition, cameraSnapDuration));
        cameraSequence.Join(Camera.transform.DORotateQuaternion(Quaternion.LookRotation(lookTarget - targetPosition), cameraSnapDuration));
    }

    public void SetPaused(bool pauseState)
    {
        isPaused = pauseState;

        if (pauseState)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
