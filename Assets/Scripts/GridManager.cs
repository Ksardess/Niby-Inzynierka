using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _grassTile, _damagetile, _blockedTile;
 
    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject _playerPrefab;
 
    private Dictionary<Vector2, Tile> _tiles;

    private PlayerController _player;
 
    void Start() {
        GenerateGrid();
        SpawnPlayer();
    }
 
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        Vector2 playerStartPos = new Vector2(_width / 2, _height / 2);

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                Vector2 tilePos = new Vector2(x, y);
                Tile tileToSpawn;

                if (tilePos == playerStartPos) {
                    tileToSpawn = _grassTile;
                } else {
                    float randomValue = UnityEngine.Random.value;
                    if (randomValue < 0.02f) {
                        tileToSpawn = _damagetile; // 1 na 10
                    } else if (randomValue < 0.1f) {
                        tileToSpawn = _blockedTile; // 3 na 10
                    } else {
                        tileToSpawn = _grassTile;
                    }
                }

                var spawnedTile = Instantiate(tileToSpawn, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[tilePos] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }
 
    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    void SpawnPlayer()
    {
        var playerInstance = Instantiate(_playerPrefab, new Vector3(_width / 2, _height / 2, 0), Quaternion.identity);
        _player = playerInstance.GetComponent<PlayerController>();
        _player.Init(this, new Vector2(_width / 2, _height / 2));

        CameraFollow cameraFollow = _cam.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = playerInstance.transform;
        }
    }
}