﻿using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour  {
    [SerializeField] TileBase[] allowedTiles = null;

    public bool Contain(TileBase tile) {
        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get() { return allowedTiles;  }

    public TileBase GetTile(string name) {
        return allowedTiles.FirstOrDefault(tile => tile.name == name);
    }

    public void AddTile(TileBase tile) {
        allowedTiles = allowedTiles.Append(tile).ToArray();
    }
}
