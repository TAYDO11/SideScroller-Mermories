using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// Ajoute ce composant sur ton PERSONNAGE
public class ItemPowerController : MonoBehaviour
{
    [Header("Course (Maj)")]
    public float runMultiplier = 2f;

    [Header("Dash (Ctrl)")]
    public float dashForce = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    // Référence vers ton script de mouvement
    // Remplace "PlayerMovement" par le vrai nom de ton script
    private PlayerMovement2D playerMovement;
    private Rigidbody2D rb;

    private bool powersEnabled = false;
    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement2D>(); 
    }

    void Update()
    {
        if (!powersEnabled) return;

        HandleRun();
        HandleDash();
    }

    void HandleRun()
    {
        if (playerMovement == null) return;

        playerMovement.speedMultiplier = Keyboard.current.leftShiftKey.isPressed
            ? runMultiplier
            : 1f;
    }

    void HandleDash()
    {
        if (!canDash || isDashing) return;
        if (!Keyboard.current.leftCtrlKey.wasPressedThisFrame) return;

        Vector2 dir = rb.linearVelocity.normalized;

        // Si immobile, dash dans la direction regardée
        if (dir == Vector2.zero)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            dir = (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
        }

        StartCoroutine(DashCoroutine(dir));
    }

    IEnumerator DashCoroutine(Vector2 direction)
    {
        isDashing = true;
        canDash = false;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void EnablePowers()
    {
        powersEnabled = true;
    }

    public void DisablePowers()
    {
        powersEnabled = false;

        if (playerMovement != null)
            playerMovement.speedMultiplier = 1f;

        if (isDashing)
        {
            StopAllCoroutines();
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 1f;
            isDashing = false;
            canDash = true;
        }
    }
}
