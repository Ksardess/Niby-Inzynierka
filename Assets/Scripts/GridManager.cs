using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _grassTile, _mountainTile;
 
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
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var randomTile = UnityEngine.Random.Range(0, 6) == 3 ? _mountainTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                spawnedTile.Init(x,y);
 
 
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
 
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f,-10);
    }
 
    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    void SpawnPlayer() {
    Vector2 startPosition = new Vector2(0, 0);

    if (GetTileAtPosition(startPosition) != null) {
        var playerInstance = Instantiate(_playerPrefab, new Vector3(startPosition.x, startPosition.y, 0), Quaternion.identity);
        _player = playerInstance.GetComponent<PlayerController>();
        _player.Init(this, startPosition);
    } else {
        Debug.LogError("Nie można zainicjalizować gracza poza gridem!");
    }
}
}
