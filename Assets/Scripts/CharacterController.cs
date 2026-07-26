using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float climbSpeed = 6f;
    [SerializeField] private float jumpForce = 16f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask[] groundLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float originalGravityScale; 

    private bool isGrounded;
    private float horizontalInput;
    private float verticalInput;
    private float coyoteTimeCounter;

    [Header("Dashing")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashCooldown = 0f;
    [SerializeField] private float dashCooldownDuration = 1f;
    [SerializeField] private float dashDuration = 0.15f;

    [Header("Drop Through")]
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask oneWayPlatformMask;

    [Header("Vine")]
    [SerializeField] private LayerMask vineLayer;
    [SerializeField] private float vineCheckRadius = 0.3f;
    [SerializeField] private bool canClimbVine = true;
    private Transform clingedVine;

    private bool isOnVine;
    private bool isClinging;

    private bool isDashing = false;
    private float dashTimer = 0f;

    private bool isKnockedback;
    private float knockbackTime = 0f;
    private float knockbackDuration = 0.5f;
    private bool isOnLadder = false;
    private bool isClimbing = false;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravityScale = rb.gravityScale;
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ladder"))
                isOnLadder = true;
        }

    private void OnTriggerExit2D(Collider2D other)
    {
            if (other.CompareTag("Ladder"))
                isOnLadder = false;
    }

    void Update()
    {
        if (isKnockedback)
        {
            knockbackTime -= Time.deltaTime;
            if (knockbackTime <= 0f)
            {
                isKnockedback = false;
            }
            return; // Skip normal movement while knockedback is active
        }

        horizontalInput = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1
        verticalInput = Input.GetAxisRaw("Vertical"); 

        isGrounded = false;
        foreach (LayerMask layer in groundLayer)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, layer))
            {
                isGrounded = true;
                break;
            }
        }

        if ((verticalInput != 0) && isOnLadder) // Assumes no jumping once off the ground
        {   
            isClimbing = true;
            rb.gravityScale = 0f; 
        }
        else if (!isOnLadder)
        {
            isClimbing = false;
            rb.gravityScale = originalGravityScale; 
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        bool holdingDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        if (Input.GetButtonDown("Jump"))
        {
            if (isClinging)
            {
                ReleaseVine();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);   // jump off
            }
            else if (holdingDown && TryDropThrough()) { /* dropped through */ }
            else if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
            }
        }

        if (dashCooldown > 0f)
            dashCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown <= 0f && !isClinging)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldown = dashCooldownDuration;

            // Dash in facing direction
            float dashDir = spriteRenderer.flipX ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset Y
            rb.AddForce(new Vector2(dashDir * dashForce, 0f), ForceMode2D.Impulse);

        }

        Collider2D vineHit = Physics2D.OverlapCircle(transform.position, vineCheckRadius, vineLayer);
        isOnVine = vineHit != null;
        if (!isOnVine) ReleaseVine();

        if (isOnVine && Input.GetKeyDown(KeyCode.W))
        {
            isClinging = true;
            rb.linearVelocity = Vector2.zero;
            clingedVine = vineHit.transform;
            transform.SetParent(clingedVine);
        }


        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        //i  have sprite 
        if (horizontalInput > 0f)
        {
            spriteRenderer.flipX = false;
            //attackHitbox.localPosition = new Vector3(0.6f, attackHitbox.localPosition.y, attackHitbox.localPosition.z);
        }
        else if (horizontalInput < 0f)
        {
            spriteRenderer.flipX = true;

        }

        if (isClinging)
        {
            rb.gravityScale = 0f;
        }
        else if ((verticalInput != 0) && isOnLadder)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
        }
        else if (!isOnLadder)
        {
            isClimbing = false;
            rb.gravityScale = originalGravityScale;
        }

    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockedback = true;
        knockbackTime = knockbackDuration;
        rb.linearVelocity = Vector2.zero; // Stop current movement before applying knockback
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (isKnockedback || isDashing) return;

        if (isClinging)
        {
            float climb = canClimbVine ? verticalInput * climbSpeed : 0f;
            rb.linearVelocity = new Vector2(0f, climb);
            return;
        }

        if (isOnLadder)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * climbSpeed);
        else
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool TryDropThrough()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            playerCollider.bounds.center, Vector2.down,
            playerCollider.bounds.extents.y + 0.2f,
            oneWayPlatformMask);

        if (hit.collider != null &&
            hit.collider.TryGetComponent(out PlatformEffector2D _))
        {
            StartCoroutine(DisableCollision(hit.collider));
            return true;
        }
        return false;  
    }

    private IEnumerator DisableCollision(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);

        float startFeetY = playerCollider.bounds.min.y;
        float elapsed = 0f, safety = 1f;

        while (playerCollider.bounds.max.y > startFeetY && elapsed < safety)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }
    private void ReleaseVine()
    {
        if (isClinging)
        {
            transform.SetParent(null, true);
            isClinging = false;
            rb.gravityScale = originalGravityScale;
        }
        clingedVine = null;
    }
}