using UnityEngine;

public class LootPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemData itemData;
    [Min(1)] public int amount = 1;

    public bool TryPickup(Inventory inventory)
    {
        if (inventory == null || itemData == null)
        {
            Debug.LogWarning("LootPickup: Inventory or ItemData is missing.");
            return false;
        }

        bool added = inventory.AddItem(itemData, amount);

        if (added)
        {
            Destroy(gameObject);
        }

        return added;
    }
}