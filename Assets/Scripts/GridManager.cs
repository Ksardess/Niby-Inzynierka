using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Dodaj to, aby używać TextMeshPro

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _grassTile, _damagetile, _blockedTile;
 
    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _basicEnemyPrefab; // Dodaj prefabrykat Basic Enemy
    [SerializeField] private GameObject _coinPrefab; // Dodaj to pole

    [SerializeField] private TextMeshProUGUI _tickText; // Dodaj referencję do elementu TextMeshPro

    private Dictionary<Vector2, Tile> _tiles;
    private List<BasicEnemy> _enemies = new List<BasicEnemy>(); // Lista przeciwników

    private PlayerController _player;
    private StatsController _playerStats; // referencja do statystyk gracza
    private int _ticks = 0; // Licznik ticków
    private bool _isPlayerTurn = true; // Flaga określająca, czy jest tura gracza

    void Start() {
        GenerateGrid();
        SpawnPlayer();
        UpdateTickText(); // Zaktualizuj tekst na początku
    }

    void SpawnPlayer()
    {
        var playerInstance = Instantiate(_playerPrefab, new Vector3(_width / 2, _height / 2, -1), Quaternion.identity);
        _player = playerInstance.GetComponent<PlayerController>();
        _player.Init(this, new Vector2(_width / 2, _height / 2));

        // przypnij StatsController jeśli istnieje na prefabie
        _playerStats = playerInstance.GetComponent<StatsController>();

        CameraFollow cameraFollow = _cam.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = playerInstance.transform;
        }
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
                        tileToSpawn = _damagetile;
                    } else if (randomValue < 0.1f) {
                        tileToSpawn = _blockedTile;
                    } else {
                        tileToSpawn = _grassTile;
                    }
                }

                var spawnedTile = Instantiate(tileToSpawn, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[tilePos] = spawnedTile;

                // Generowanie Basic Enemy na 1 na 100 Tile'i
                if (UnityEngine.Random.value < 0.01f && tileToSpawn != _blockedTile && tileToSpawn != _damagetile && tilePos != playerStartPos) {
                    var enemyInstance = Instantiate(_basicEnemyPrefab, new Vector3(x, y, -1), Quaternion.identity);
                    var basicEnemy = enemyInstance.GetComponent<BasicEnemy>();
                    basicEnemy.Init(this, tilePos);
                    _enemies.Add(basicEnemy); // Dodaj przeciwnika do listy
                }

                // GENEROWANIE COIN NA LOSOWYCH GRASS TILE
                if (tileToSpawn == _grassTile && tilePos != playerStartPos && UnityEngine.Random.value < 0.05f) // np. 5% szansy
                {
                    Instantiate(_coinPrefab, new Vector3(x + 1f, y + 1f, -0.02f), Quaternion.identity);
                    // Pozycja (x+1, y+1) ustawia coin na środku tile'a
                }
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public void Tick()
    {
        _ticks++;
        UpdateTickText(); // Zaktualizuj tekst po każdym ticku

        // przekazujemy Tick do PlayerStats aby mogła zregenerować energię (jeśli warunki spełnione)
        if (_playerStats != null)
        {
            _playerStats.OnGridTick(_ticks);
        }

        _isPlayerTurn = false; // Zmień na turę przeciwnika
        StartCoroutine(EnemyTurn());
        //Debug.Log("Ticks: " + _ticks);
    }

    private IEnumerator EnemyTurn()
    {
        _isPlayerTurn = false;
        yield return new WaitForSeconds(0.5f); // Opóźnienie przed ruchem przeciwników

        foreach (var enemy in _enemies)
        {
            if (enemy != null) // Sprawdź, czy enemy nie jest null
            {
                enemy.OnTick();
                yield return new WaitForSeconds(0.01f); // Opóźnienie między ruchami przeciwników
            }
        }

        _isPlayerTurn = true;
    }

    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }

    private void UpdateTickText()
    {
        if (_tickText != null)
        {
            _tickText.text = "Ticks: " + _ticks;
        }
    }
}