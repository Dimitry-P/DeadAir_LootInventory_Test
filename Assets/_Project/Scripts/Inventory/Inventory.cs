
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;
    [Header("Inventory Settings")]
    [Min(0.1f)]
    public float maxWeight = 20f;

    [Header("Inventory Contents")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    public float CurrentWeight
    {
        get
        {
            float totalWeight = 0f;

            foreach (InventorySlot slot in slots)
            {
                if (slot.item != null)
                {
                    totalWeight += slot.item.weight * slot.quantity;
                }
            }

            return totalWeight;
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
        {
            Debug.LogWarning("Cannot add item: invalid item or amount.");
            return false;
        }

        float incomingWeight = item.weight * amount;
        
        Debug.Log(
            $"Weight={item.weight}, " +
            $"Amount={amount}, " +
            $"IncomingWeight={incomingWeight}, " +
            $"CurrentWeight={CurrentWeight}, " +
            $"MaxWeight={maxWeight}"
        );

        Debug.Log(
            $"{CurrentWeight + incomingWeight > maxWeight}"
        );
        if (CurrentWeight + incomingWeight > maxWeight)
        {
            Debug.LogWarning(
                $"Cannot pick up {item.itemName}: inventory is full! " +
                $"Current weight: {CurrentWeight}/{maxWeight}."
            );

            return false;
        }

        int remainingAmount = amount;

        // Fill existing stacks first.
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                int availableSpace = item.maxStack - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, remainingAmount);

                slot.quantity += amountToAdd;
                remainingAmount -= amountToAdd;

                if (remainingAmount <= 0)
                {
                    break;
                }
            }
        }

        // Create new stacks if necessary.
        while (remainingAmount > 0)
        {
            int amountToAdd = Mathf.Min(item.maxStack, remainingAmount);

            slots.Add(new InventorySlot
            {
                item = item,
                quantity = amountToAdd
            });

            remainingAmount -= amountToAdd;
        }

        Debug.Log(
            $"Picked up {amount} x {item.itemName}. " +
            $"Inventory weight: {CurrentWeight}/{maxWeight}."
        );
        OnInventoryChanged?.Invoke();
        return true;
    }
    
    public bool DismantleItem(ItemData item)
    {
        if (item == null || item.dismantleResults == null ||
            item.dismantleResults.Count == 0)
        {
            Debug.LogWarning("Cannot dismantle item: no dismantling results.");
            return false;
        }

        // Find one item in the inventory.
        InventorySlot slotToRemove = slots.Find(
            slot => slot.item == item && slot.quantity > 0
        );

        if (slotToRemove == null)
        {
            Debug.LogWarning($"Cannot dismantle {item.itemName}: item not found.");
            return false;
        }

        // Validate dismantling results and calculate their total weight.
        float resultsWeight = 0f;

        foreach (DismantleResult result in item.dismantleResults)
        {
            if (result == null || result.item == null || result.amount <= 0)
            {
                Debug.LogWarning("Cannot dismantle item: invalid dismantling result.");
                return false;
            }

            resultsWeight += result.item.weight * result.amount;
        }

        // Check inventory weight after dismantling.
        float resultingWeight = CurrentWeight - item.weight + resultsWeight;

        if (resultingWeight > maxWeight)
        {
            Debug.LogWarning(
                $"Cannot dismantle {item.itemName}: inventory weight limit exceeded."
            );
            return false;
        }

        // Remove one item from the inventory.
        slotToRemove.quantity--;

        if (slotToRemove.quantity <= 0)
        {
            slots.Remove(slotToRemove);
        }

        // Add dismantling results.
        foreach (DismantleResult result in item.dismantleResults)
        {
            AddItem(result.item, result.amount);
        }

        Debug.Log(
            $"Dismantled {item.itemName}. " +
            $"Inventory weight: {CurrentWeight}/{maxWeight}."
        );

        return true;
    }
}
