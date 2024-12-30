using UnityEngine;

public class DamageTile : Tile
{
     public void OnPlayerEnter(Player player)
    {
        Debug.Log("Player wszedł na MountainTile");
        player.TakeDamage(10);
        Debug.Log("Otrzymałeś obrażenia: 10");
    }
}
