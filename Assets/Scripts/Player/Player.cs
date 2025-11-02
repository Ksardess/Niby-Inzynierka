using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Vector2 _currentPosition;

    private HealthController healthController;
    private GridManager gridManager; // Referencja do GridManager
    private StatsController playerStats; // referencja do statystyk


    void Start()
    {
        healthController = GetComponent<HealthController>();
        gridManager = FindFirstObjectByType<GridManager>(); // Znajdź GridManager w scenie
        playerStats = GetComponent<StatsController>();
        _currentPosition = transform.position;
    }

    void Update()
    {
        _currentPosition = transform.position; // Aktualizuj pozycję gracza

        // SPACJA uruchamia Tick()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gridManager != null)
            {
                gridManager.Tick();
            }
        }

        if (Input.GetMouseButtonDown(1)) // PPM
        {
            if (gridManager != null && gridManager.IsPlayerTurn()) // Sprawdź, czy jest tura gracza
            {
                BasicAttack();
            }
            else
            {
                Debug.Log("Nie możesz atakować w turze przeciwnika!");
            }
        }
    }

    private void BasicAttack()
    {
        // Dodaj blokadę ataku jeśli gracz nie żyje
        if (healthController != null && healthController.CurrentHealth <= 0)
        {
            Debug.Log("Nie możesz atakować, jesteś martwy!");
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (var direction in directions)
        {
            Vector2 adjacentPosition = _currentPosition + direction;
            if (Vector2.Distance(mousePosition, adjacentPosition) < 0.75f)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, 0.1f);
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        // zużyj 5 energii tylko jeśli atak ma się wykonać
                        if (playerStats != null && !playerStats.TryUseEnergy(5))
                        {
                            Debug.Log("Nie masz wystarczająco energii!");
                            return;
                        }

                        // oznacz, że gracz atakował w tej turze (zablokuje regen)
                        if (playerStats != null)
                        {
                            playerStats.MarkAttackedThisTurn();
                        }

                        if (animator != null)
                        {
                            animator.SetTrigger("Attack1");
                        }

                        HealthController enemyHealth = collider.GetComponent<HealthController>();
                        if (enemyHealth == null && collider.transform.parent != null)
                        {
                            enemyHealth = collider.transform.parent.GetComponent<HealthController>();
                        }
                        if (enemyHealth != null)
                        {
                            int damage = (playerStats != null) ? playerStats.BaseDamage : 25;
                            // Rzut krytyka — jeśli się uda, obrażenia x2
                            if (playerStats != null && playerStats.RollCrit())
                            {
                                damage *= 2;
                                Debug.Log("Critical hit! Damage: " + damage);
                            }

                            enemyHealth.TakeDamage(damage);
                        }

                        // Po wykonaniu ataku przejdź do tury przeciwnika
                        if (gridManager != null)
                        {
                            gridManager.Tick();
                        }

                        return;
                    }
                }
            }
        }
    }
}