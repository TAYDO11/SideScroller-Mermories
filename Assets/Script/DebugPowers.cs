using UnityEngine;

// Script de débogage temporaire - supprime le une fois que tout fonctionne
public class DebugPowers : MonoBehaviour
{
    void Start()
    {
        var pm = GetComponent<PlayerMovement2D>();
        var ipc = GetComponent<ItemPowerController>();
        var him = GetComponent<HeldItemManager>();

        Debug.Log("=== DEBUG POWERS ===");
        Debug.Log("PlayerMovement2D trouvé : " + (pm != null));
        Debug.Log("ItemPowerController trouvé : " + (ipc != null));
        Debug.Log("HeldItemManager trouvé : " + (him != null));

        if (pm != null)
        {
            Debug.Log("speedMultiplier = " + pm.speedMultiplier);
            Debug.Log("dashUnlocked = " + pm.dashUnlocked);
        }
    }

    void Update()
    {
        var pm = GetComponent<PlayerMovement2D>();
        var ipc = GetComponent<ItemPowerController>();

        // Affiche l'état toutes les secondes
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log("--- UPDATE ---");
            if (pm != null)
            {
                Debug.Log("speedMultiplier = " + pm.speedMultiplier);
                Debug.Log("dashUnlocked = " + pm.dashUnlocked);
            }
            if (ipc != null)
                Debug.Log("ItemPowerController présent");
        }
    }
}
