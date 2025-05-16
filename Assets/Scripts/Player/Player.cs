using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Vector2 _currentPosition;

    private HealthController healthController;

    void Start()
    {
        healthController = GetComponent<HealthController>();
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

                        return;
                    }
                }
            }
        }
    }
}