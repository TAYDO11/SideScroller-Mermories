using UnityEngine;

// Ajoute ce composant sur le prefab de l'item VOL
public class ItemFlightController : MonoBehaviour
{
    [Header("Vol")]
    public float flySpeed = 6f;
    public float flyAcceleration = 10f;

    private Rigidbody2D rb;
    private float normalGravityScale;
    private bool flightEnabled = false;
    private bool isFlying = false;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
            normalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (!flightEnabled || rb == null) return;

        // Active le vol en maintenant Espace
        isFlying = Input.GetKey(KeyCode.Space);

        if (isFlying)
            rb.gravityScale = 0f;
        else
            rb.gravityScale = normalGravityScale;
    }

    void FixedUpdate()
    {
        if (!flightEnabled || !isFlying || rb == null) return;

        // Déplacement dans toutes les directions
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        Vector2 targetVelocity = direction * flySpeed;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, flyAcceleration * Time.fixedDeltaTime);
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
        isFlying = false;
        if (rb != null)
            rb.gravityScale = normalGravityScale;
    }
}