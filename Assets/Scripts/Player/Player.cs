using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Vector2 _currentPosition;

    private HealthController healthController;
    private GridManager gridManager; // Referencja do GridManager

    void Start()
    {
        healthController = GetComponent<HealthController>();
        gridManager = FindObjectOfType<GridManager>(); // Znajdź GridManager w scenie
        _currentPosition = transform.position;
    }

    void Update()
    {
        _currentPosition = transform.position; // Aktualizuj pozycję gracza

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
                        if (animator != null)
                        {
                            animator.SetTrigger("Attack1");
                        }

                        HealthController enemyHealth = collider.GetComponent<HealthController>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeDamage(25); // Zadaj 25 obrażeń
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