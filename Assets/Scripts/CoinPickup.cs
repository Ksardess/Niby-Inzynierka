using UnityEngine;
using InventorySystem;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private string inventoryName = "PlayerInventory"; // Nazwa ekwipunku gracza
    [SerializeField] private string coinItemType = "Coin"; // Typ itemu zgodny z InventoryController

    private void OnMouseDown()
    {
        // Znajdź gracza w scenie
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Sprawdź odległość gracza od coina
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > 2.0f)
        {
            Debug.Log("Za daleko, aby podnieść monetę!");
            return;
        }

        // Dodaj monetę do ekwipunku
        if (InventoryController.instance != null)
        {
            InventoryController.instance.AddItem(inventoryName, coinItemType, 1);
            Debug.Log("Moneta dodana do ekwipunku!");
        }
        else
        {
            Debug.LogWarning("Brak InventoryController w scenie!");
        }

        // Usuń monetę z planszy
        Destroy(gameObject);
    }
}