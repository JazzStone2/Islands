using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float SprintSpeed = 10f;
    public float SprintCoolDown = 5f;
    public float sprintLength = 10f;

    private float currentSpeed;
    private bool isSprinting = false;
    private float sprintTimeLeft;
    private float sprintCooldownTimer;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = Speed;
    }

    void Update()
    {
        HandleInput();
        HandleSprinting();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void HandleMovement()
    {
        // Normalize the movement vector to ensure consistent speed in all directions
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }

        // Apply the normalized movement vector with the current speed
        rb.linearVelocity = movement * currentSpeed;
    }

    void HandleSprinting()
    {
        // Check if the Shift key is pressed and sprint conditions are met
        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting && sprintCooldownTimer <= 0f)
        {
            isSprinting = true;
            sprintTimeLeft = Mathf.Clamp(sprintTimeLeft, 0f, sprintLength); // Ensure sprintTimeLeft doesn't exceed the limit
            currentSpeed = SprintSpeed;
        }

        // Handle sprinting logic
        if (isSprinting)
        {
            sprintTimeLeft -= Time.deltaTime;

            // Stop sprinting if the sprint time runs out or if the Shift key is released
            if (sprintTimeLeft <= 0f || !Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = false;
                currentSpeed = Speed;

                // Start cooldown only if sprint runs out naturally
                if (sprintTimeLeft <= 0f)
                {
                    sprintCooldownTimer = SprintCoolDown;
                }
            }
        }
        else
        {
            // Regenerate sprint time when not sprinting
            if (sprintTimeLeft < sprintLength)
            {
                sprintTimeLeft += Time.deltaTime; // Adjust the regeneration speed by multiplying Time.deltaTime with a factor if needed
                sprintTimeLeft = Mathf.Clamp(sprintTimeLeft, 0f, sprintLength); // Clamp to ensure it doesn't exceed the max length
            }

            // Handle sprint cooldown logic
            if (sprintCooldownTimer > 0f)
            {
                sprintCooldownTimer -= Time.deltaTime;
            }
        }
    }

    void UpdateAnimation()
    {
        // Determine if the player is moving
        bool isMoving = movement.sqrMagnitude > 0.01f;

        // Set parameters for running animations
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("IsSprinting", isSprinting);

        if (isMoving)
        {
            // For running, face the direction of movement
            Vector3 scale = transform.localScale;
            if (movement.x != 0)
            {
                scale.x = movement.x > 0 ? 1 : -1; // Flip sprite based on movement direction
            }
            transform.localScale = scale;
        }
        else
        {
            // For idle, face the direction of the mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Set animator parameters for mouse position
            animator.SetFloat("MouseHorizontal", direction.x);
            animator.SetFloat("MouseVertical", direction.y);

            // Optionally, flip sprite based on mouse direction
            if (direction.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = direction.x > 0 ? 1 : -1;
                transform.localScale = scale;
            }
        }
    }
}
