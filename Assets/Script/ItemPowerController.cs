using UnityEngine;
using UnityEngine.InputSystem;

// Ajoute ce composant sur ton PERSONNAGE
public class ItemPowerController : MonoBehaviour
{
    [Header("Course (Maj gauche)")]
    public float runMultiplier = 2f;

    private PlayerMovement2D playerMovement;
    private bool powersEnabled = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement2D>();
    }

    void Update()
    {
        if (!powersEnabled || playerMovement == null) return;

        // Course : Maj gauche
        playerMovement.speedMultiplier = Keyboard.current.leftShiftKey.isPressed
            ? runMultiplier
            : 1f;
    }

    public void EnablePowers()
    {
        powersEnabled = true;
        if (playerMovement != null)
            playerMovement.dashUnlocked = true;
    }

    public void DisablePowers()
    {
        powersEnabled = false;
        if (playerMovement != null)
        {
            playerMovement.speedMultiplier = 1f;
            playerMovement.dashUnlocked = false;
        }
    }
}
