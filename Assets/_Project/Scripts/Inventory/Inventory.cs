
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

        return true;
    }
}
