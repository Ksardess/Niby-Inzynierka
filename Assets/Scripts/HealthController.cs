using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

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

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                Destroy(gameObject); // fallback, jeśli nie ma animatora
            }
            Debug.Log($"{gameObject.name} został zniszczony (po animacji Death)");
            // NIE wywołuj Destroy tutaj, poczekaj na Animation Event!
        }
    }

    // Dodaj tę metodę do wywołania przez Animation Event na końcu animacji Death
    public void DestroyAfterDeathAnimation()
    {
        Destroy(gameObject);
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