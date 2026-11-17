using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Consumable,
    RepairResource,
    Junk,
    Weapon
}

public enum PickupMode
{
    Automatic,
    Manual
}

[Serializable]
public class DismantleResult
{
    public ItemData item;
    [Min(1)] public int amount = 1;
}

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Dead Air/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public ItemCategory category;

    [Header("Inventory Settings")]
    [Min(0.01f)] public float weight = 1f;
    [Min(1)] public int maxStack = 1;

    [Header("Pickup Settings")]
    public PickupMode pickupMode = PickupMode.Automatic;

    [Header("Dismantling")]
    public List<DismantleResult> dismantleResults =
        new List<DismantleResult>();
}