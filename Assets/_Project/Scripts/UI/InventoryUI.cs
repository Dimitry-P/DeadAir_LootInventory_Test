
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory References")]
    [SerializeField] private Inventory inventory;

    [Header("UI References")]
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject itemEntryPrefab;
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private Button dismantleButton;

    private ItemData selectedItem;

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshUI;
        }

        if (dismantleButton != null)
        {
            dismantleButton.onClick.AddListener(DismantleSelectedItem);
        }
    }

    private void Start()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshUI;
        }

        if (dismantleButton != null)
        {
            dismantleButton.onClick.RemoveListener(DismantleSelectedItem);
        }
    }

    public void RefreshUI()
    {
        if (inventory == null || itemListParent == null ||
            itemEntryPrefab == null || weightText == null)
        {
            return;
        }

        // Clear old item entries.
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        // Create an entry for each inventory slot.
        foreach (InventorySlot slot in inventory.slots)
        {
            if (slot.item == null)
                continue;

            GameObject entry = Instantiate(
                itemEntryPrefab,
                itemListParent
            );

            TMP_Text entryText = entry.GetComponentInChildren<TMP_Text>();

            if (entryText != null)
            {
                entryText.text = $"{slot.item.itemName} ({slot.quantity})";
            }

            Button entryButton = entry.GetComponent<Button>();

            if (entryButton != null)
            {
                ItemData item = slot.item;
                entryButton.onClick.AddListener(() => SelectItem(item));
            }
        }

        // Update inventory weight.
        weightText.text =
            $"Weight: {inventory.CurrentWeight} / {inventory.maxWeight}";

        // Enable dismantling only when a dismantlable item is selected.
        UpdateDismantleButton();
    }

    private void SelectItem(ItemData item)
    {
        selectedItem = item;
        UpdateDismantleButton();

        Debug.Log($"Selected item: {item.itemName}");
    }

    private void UpdateDismantleButton()
    {
        if (dismantleButton == null)
            return;

        dismantleButton.interactable =
            selectedItem != null &&
            selectedItem.dismantleResults != null &&
            selectedItem.dismantleResults.Count > 0;
    }

    private void DismantleSelectedItem()
    {
        if (inventory == null || selectedItem == null)
            return;

        bool dismantled = inventory.DismantleItem(selectedItem);

        if (dismantled)
        {
            selectedItem = null;
            RefreshUI();
        }
    }
}
