using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector2 _currentPosition;
    private GridManager _gridManager;
    private HealthController healthController; // Dodaj referencję

    public void Init(GridManager gridManager, Vector2 startPosition) {
        _gridManager = gridManager;
        _currentPosition = startPosition;

        // Ustaw gracza na początkowej pozycji z przesunięciem Y o -0.3
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y - 0.3f, -1);

        // Pobierz HealthController
        healthController = GetComponent<HealthController>();
    }

    void Start() {
        // Jeśli Init nie został wywołany, pobierz HealthController tutaj
        if (healthController == null)
            healthController = GetComponent<HealthController>();
    }

    void Update() {
        // Blokada ruchu jeśli gracz nie żyje
        if (healthController != null && healthController.CurrentHealth <= 0)
            return;

        if (_gridManager.IsPlayerTurn())
        {
            HandleMovement();
        }
    }

    private void HandleMovement() {
        // Odbierz klawisze kierunkowe
        Vector2 movement = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) movement = Vector2.up;    // Ruch w górę
        if (Input.GetKeyDown(KeyCode.S)) movement = Vector2.down;  // Ruch w dół
        if (Input.GetKeyDown(KeyCode.A)) movement = Vector2.left;  // Ruch w lewo
        if (Input.GetKeyDown(KeyCode.D)) movement = Vector2.right; // Ruch w prawo

        if (movement != Vector2.zero) {
            Vector2 newPosition = _currentPosition + movement;

            // Sprawdź, czy nowa pozycja jest w gridzie i nie jest zajęta
            Tile tile = _gridManager.GetTileAtPosition(newPosition);
            if (tile != null && !(tile is BlockedTile) && !IsPositionOccupiedByEnemy(newPosition)) {
                MoveToTile(newPosition);
                _gridManager.Tick(); // Informuj GridManager o ruchu
            }
        }
    }

    private bool IsPositionOccupiedByEnemy(Vector2 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders) {
            if (collider.GetComponent<BasicEnemy>() != null) {
                return true;
            }
        }
        return false;
    }

    private void MoveToTile(Vector2 newPosition) {
        _currentPosition = newPosition;
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y - 0.3f, -1); // Ustaw współrzędną Y z przesunięciem o -0.3
    }

}