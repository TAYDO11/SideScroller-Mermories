using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// Ajoute ce composant sur ton PERSONNAGE
public class HeldItemManager : MonoBehaviour
{
    [Header("Référence main")]
    public Transform handTransform;

    private GameObject currentHeldItem;
    private ItemAuraController currentAura;
    private ItemPowerController powerController;
    private ItemFlightController currentFlight;
    private SpriteRenderer playerSprite;
    private int currentSlot = -1;

    void Start()
    {
        powerController = GetComponent<ItemPowerController>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Clavier : touches 1, 2, 3, 4
        for (int i = 0; i < 4; i++)
        {
            if (Keyboard.current[Key.Digit1 + i].wasPressedThisFrame)
            {
                SelectSlot(i);
                return;
            }
        }

        // Flip de l'item avec le sprite du personnage
        if (currentHeldItem != null && playerSprite != null)
        {
            SpriteRenderer itemSprite = currentHeldItem.GetComponent<SpriteRenderer>();
            if (itemSprite != null)
                itemSprite.flipX = playerSprite.flipX;
        }

        // Manette PS : L1 + X = slot 0, L1 + carré = slot 1
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.leftShoulder.isPressed && gamepad.buttonSouth.wasPressedThisFrame)
                SelectSlot(0);
            if (gamepad.leftShoulder.isPressed && gamepad.buttonWest.wasPressedThisFrame)
                SelectSlot(1);
        }
    }

    public void SelectSlot(int index)
    {
        if (!inventory.instance.HasItemInSlot(index))
        {
            Debug.Log("Slot " + index + " vide.");
            return;
        }

        // Toggle : déséquipe si on rappuie sur le même slot
        if (currentSlot == index)
        {
            UnequipItem();
            return;
        }

        Item item = inventory.instance.GetItemInSlot(index);

        if (item.heldPrefab == null)
        {
            Debug.LogWarning("Pas de heldPrefab sur l'item " + index);
            return;
        }

        EquipItem(item.heldPrefab, index);
    }

    void EquipItem(GameObject prefab, int slotIndex)
    {
        if (currentHeldItem != null)
        {
            if (currentAura != null) currentAura.DisableAura();
            if (powerController != null) powerController.DisablePowers();
            if (currentFlight != null) currentFlight.DisableFlight();
            Destroy(currentHeldItem);
        }

        currentSlot = slotIndex;
        currentHeldItem = Instantiate(prefab, handTransform.position, Quaternion.identity);
        currentHeldItem.transform.SetParent(handTransform);
        currentHeldItem.transform.localPosition = Vector3.zero;

        // --- Pouvoir LUMIERE ---
        currentAura = currentHeldItem.GetComponent<ItemAuraController>();
        if (currentAura != null)
            StartCoroutine(EnableAuraNextFrame(currentAura));

        // --- Pouvoir DASH + COURSE ---
        ItemGrantsPowers grants = currentHeldItem.GetComponent<ItemGrantsPowers>();
        if (grants != null && grants.grantsDashAndRun && powerController != null)
            powerController.EnablePowers();

        // --- Pouvoir VOL ---
        currentFlight = currentHeldItem.GetComponent<ItemFlightController>();
        if (currentFlight != null)
            currentFlight.EnableFlight();
    }

    IEnumerator EnableAuraNextFrame(ItemAuraController aura)
    {
        yield return null;
        aura.EnableAura();
    }

    public void UnequipItem()
    {
        if (currentAura != null) currentAura.DisableAura();
        if (powerController != null) powerController.DisablePowers();
        if (currentFlight != null) currentFlight.DisableFlight();
        if (currentHeldItem != null) Destroy(currentHeldItem);

        currentHeldItem = null;
        currentAura = null;
        currentFlight = null;
        currentSlot = -1;
    }
}
