using UnityEngine;

public class BlockedTile : Tile
{
    [SerializeField] private GameObject[] _variants; // Tablica wariantów

    public override void Init(int x, int y) {
        // Wybierz losowy wariant
        if (_variants.Length > 0) {
            int randomIndex = Random.Range(0, _variants.Length);
            GameObject variant = Instantiate(_variants[randomIndex], transform.position, Quaternion.identity);
            variant.transform.SetParent(transform);
        }
    }
}