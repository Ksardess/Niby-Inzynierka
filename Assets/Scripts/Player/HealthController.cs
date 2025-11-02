using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] private HealthBar healthBar; // Opcjonalne, dla gracza
    [SerializeField] private TextMeshProUGUI healthText; // przypisz w Inspectorze

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }

        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        // Jeśli obiekt posiada StatsController (najpewniej gracz), najpierw spróbuj uniknąć obrażeń (Elusion),
        // potem oblicz redukcję przez pancerz.
        int finalDamage = damage;
        if (TryGetComponent<StatsController>(out var ps))
        {
            // elusion — szansa na uniknięcie całych obrażeń
            if (ps.RollElusion())
            {
                Debug.Log($"{gameObject.name} uniknął obrażeń dzięki Elusion!");
                return;
            }
            finalDamage = ps.CalculateDamageAfterArmor(damage);
        }

        currentHealth -= finalDamage;
        Debug.Log($"{gameObject.name} otrzymał obrażenia: {damage} (po pancerzu: {finalDamage}), pozostałe zdrowie: {currentHealth}");

        UpdateHealthText();

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

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText == null) return;
        healthText.text = $"{Mathf.Max(0, currentHealth)}/{maxHealth}";
    }
}