using UnityEngine;

public class DamageTile : Tile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdź, czy obiekt, który wszedł na pole, ma komponent HealthController
        HealthController healthController = other.GetComponent<HealthController>();
        if (healthController != null)
        {
            // Zadaj 10 obrażeń obiektowi
            healthController.TakeDamage(10);
            Debug.Log($"{other.gameObject.name} wszedł na DamageTile i otrzymał 10 obrażeń.");
        }
    }
}