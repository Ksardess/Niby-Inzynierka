using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector2 _currentPosition;
    private GridManager _gridManager;

    public void Init(GridManager gridManager, Vector2 startPosition) {
        _gridManager = gridManager;
        _currentPosition = startPosition;

        // Ustaw gracza na początkowej pozycji
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y, -1);
    }

    void Update() {
        HandleMovement();
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

            // Sprawdź, czy nowa pozycja jest w gridzie
            Tile tile = _gridManager.GetTileAtPosition(newPosition);
            if (tile != null && !(tile is BlockedTile)) {
                MoveToTile(newPosition);
                _gridManager.Tick(); // Informuj GridManager o ruchu
            }
        }
    }

    private void MoveToTile(Vector2 newPosition) {
        _currentPosition = newPosition;
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.TryGetComponent<DamageTile>(out var mountainTile)) {
            mountainTile.OnPlayerEnter(GetComponent<Player>());
        }
    }
}