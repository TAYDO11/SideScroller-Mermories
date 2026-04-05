using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundAcceleration = 15f;
    [SerializeField] private float airAcceleration = 4f;

    [Header("Saut")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash (débloqué par item)")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [SerializeField] private Animator animator;

    // Utilisés par ItemPowerController
    [HideInInspector] public float speedMultiplier = 1f;
    [HideInInspector] public bool dashUnlocked = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;
    private bool dashRequested;
    private bool isDashing;
    private float dashTimer;
    private float cooldownTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cooldownTimer = 0f;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
            jumpRequested = true;

       

        // Dash (Ctrl, seulement si item équipé)
        if (dashUnlocked && Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0f && !isDashing)
        {
            dashRequested = true;
        }

        // Flip du sprite (sans toucher au scale pour éviter les warnings collider)
        if (horizontalInput > 0 && sr != null)
            sr.flipX = false;
        else if (horizontalInput < 0 && sr != null)
            sr.flipX = true;

        // Animator
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void FixedUpdate()
    {
        // --- Dash ---
        if (dashRequested)
        {
            isDashing = true;
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown;
            dashRequested = false;

            float dir = horizontalInput != 0 ? horizontalInput : (sr != null && sr.flipX ? -1f : 1f);
            rb.linearVelocity = new Vector2(dir * dashForce, 0f);
        }

        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f) isDashing = false;
            return;
        }

        // Cooldown dash
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;

        // --- Déplacement (avec speedMultiplier pour la course) ---
        float targetSpeed = horizontalInput * moveSpeed * speedMultiplier;
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        // --- Saut ---
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}
