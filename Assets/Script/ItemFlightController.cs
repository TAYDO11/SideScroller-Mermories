using UnityEngine;

// Ajoute ce composant sur le prefab de l'item VOL
public class ItemFlightController : MonoBehaviour
{
    [Header("Vol / Planer")]
    public float glideGravityScale = 0.2f;  // Gravité réduite pendant le vol
    public float glideMaxFallSpeed = -2f;   // Vitesse de chute max en planant

    private Rigidbody2D rb;
    private float normalGravityScale;
    private bool flightEnabled = false;
    private bool isGliding = false;

    void Start()
    {
        // Récupère le Rigidbody2D sur le personnage (parent)
        rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
            normalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (!flightEnabled || rb == null) return;

        // Planer : Espace maintenu + en l'air
        bool spaceHeld = Input.GetKey(KeyCode.Space);
        bool isGrounded = rb.linearVelocity.y == 0f; // simplifié

        if (spaceHeld && !isGrounded)
        {
            isGliding = true;
            rb.gravityScale = glideGravityScale;

            // Limite la vitesse de chute
            if (rb.linearVelocity.y < glideMaxFallSpeed)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, glideMaxFallSpeed);
        }
        else
        {
            isGliding = false;
            rb.gravityScale = normalGravityScale;
        }
    }

    public void EnableFlight()
    {
        flightEnabled = true;
        if (rb != null)
            normalGravityScale = rb.gravityScale;
    }

    public void DisableFlight()
    {
        flightEnabled = false;
        isGliding = false;
        if (rb != null)
            rb.gravityScale = normalGravityScale;
    }
}
