using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private Transform _player;
    private GridManager _gridManager;
    private Vector2 _currentPosition;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
    }

    public void Init(GridManager gridManager, Vector2 startPosition)
    {
        _gridManager = gridManager;
        _currentPosition = startPosition;
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y - 0.3f, -1);
    }

    public void OnTick()
    {
        if (_player != null)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (_player == null) return;

        Vector2 direction = (_player.position - transform.position).normalized;
        Vector2 movement = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            movement = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            movement = direction.y > 0 ? Vector2.up : Vector2.down;
        }

        Vector2 newPosition = _currentPosition + movement;

        Tile tile = _gridManager.GetTileAtPosition(newPosition);
        if (tile != null && !(tile is BlockedTile) && !IsPositionOccupied(newPosition))
        {
            MoveToTile(newPosition);
        }
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject && (collider.GetComponent<Player>() != null || collider.GetComponent<BasicEnemy>() != null))
            {
                return true;
            }
        }
        return false;
    }

    private void MoveToTile(Vector2 newPosition)
    {
        _currentPosition = newPosition;
        transform.position = new Vector3(_currentPosition.x, _currentPosition.y - 0.3f, -1);
    }
}