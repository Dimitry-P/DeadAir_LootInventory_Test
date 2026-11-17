
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootInteractor : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRadius = 2.5f;
    public KeyCode pickupKey = KeyCode.E;

    private Inventory inventory;
    private readonly HashSet<LootPickup> attemptedAutomaticPickups =
        new HashSet<LootPickup>();

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("PlayerLootInteractor requires an Inventory component.");
        }
    }

    private void Update()
    {
        if (inventory == null)
            return;

        Collider[] nearbyColliders =
            Physics.OverlapSphere(transform.position, pickupRadius);

        LootPickup nearestManualPickup = null;
        float nearestDistance = float.MaxValue;

        HashSet<LootPickup> currentlyNearby =
            new HashSet<LootPickup>();

        foreach (Collider nearbyCollider in nearbyColliders)
        {
            LootPickup loot = nearbyCollider.GetComponentInParent<LootPickup>();

            if (loot == null || loot.itemData == null)
                continue;

            currentlyNearby.Add(loot);

            if (loot.itemData.pickupMode == PickupMode.Automatic)
            {
                if (!attemptedAutomaticPickups.Contains(loot))
                {
                    attemptedAutomaticPickups.Add(loot);
                    loot.TryPickup(inventory);
                }
            }
            else if (loot.itemData.pickupMode == PickupMode.Manual)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    loot.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestManualPickup = loot;
                }
            }
        }

        // Allow automatic pickup to be attempted again
        // after the player leaves the item's pickup radius.
        attemptedAutomaticPickups.RemoveWhere(
            loot => loot == null || !currentlyNearby.Contains(loot)
        );

        if (Input.GetKeyDown(pickupKey) && nearestManualPickup != null)
        {
            nearestManualPickup.TryPickup(inventory);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
