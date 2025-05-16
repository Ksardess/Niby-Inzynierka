using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private HealthBar healthBar; // Opcjonalne, dla gracza

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} otrzymał obrażenia: {damage}, pozostałe zdrowie: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} został zniszczony");
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log($"{gameObject.name} został uleczony o: {amount}, obecne zdrowie: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}