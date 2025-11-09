using UnityEngine;
using InventorySystem;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string inventoryName = "PlayerInventory"; // Nazwa ekwipunku gracza
    [SerializeField] private string itemType = "Coin"; // Typ itemu zgodny z InventoryController
    [SerializeField] private int amount = 1; // Ilość podnoszonego przedmiotu
    [SerializeField] private float pickupRange = 2.0f; // Zasięg podnoszenia

    private void OnMouseDown()
    {
        // Znajdź gracza w scenie
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Sprawdź odległość gracza od przedmiotu
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > pickupRange)
        {
            Debug.Log("Za daleko, aby podnieść przedmiot!");
            return;
        }

        // Dodaj przedmiot do ekwipunku
        if (InventoryController.instance != null)
        {
            InventoryController.instance.AddItem(inventoryName, itemType, amount);
            Debug.Log($"{itemType} x{amount} dodano do ekwipunku!");
            ActionLogManager.Instance?.AddLog($"-{itemType} x{amount} added to inventory");
        }
        else
        {
            Debug.LogWarning("Brak InventoryController w scenie!");
        }

        // Wywołaj Tick po podniesieniu przedmiotu
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            gridManager.Tick();
        }

        // Usuń przedmiot z planszy
        Destroy(gameObject);
    }
}