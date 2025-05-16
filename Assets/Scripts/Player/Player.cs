using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Animator animator; // Dodaj referencję do Animatora

    private Vector2 _currentPosition;

    void Start()
    {
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        _currentPosition = transform.position;
    }

    void Update()
    {
        _currentPosition = transform.position; // Aktualizuj pozycję gracza

        if (Input.GetMouseButtonDown(1)) // PPM
        {
            BasicAttack();
        }
    }

    private void BasicAttack()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (var direction in directions)
        {
            Vector2 adjacentPosition = _currentPosition + direction;
            if (Vector2.Distance(mousePosition, adjacentPosition) < 0.75f) // Zwiększ odległość do 0.5f
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, 0.1f);
                bool enemyFound = false;
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        enemyFound = true;
                        // Dodaj animację ataku
                        if (animator != null)
                        {
                            animator.SetTrigger("Attack1");
                        }

                        BasicEnemy enemy = collider.GetComponent<BasicEnemy>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(25); // Zadaj 25 obrażeń
                            Debug.Log("Zadano 25 obrażeń");
                        }
                        return;
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Otrzymałeś obrażenia: " + damage);

        healthBar.SetHealth(currentHealth);
    }
}