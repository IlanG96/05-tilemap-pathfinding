using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    bool isCutter = false;
    [SerializeField] TileBase SpecialTiles = null;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }
    

void Update() {
    Vector3 newPosition = NewPosition();
    TileBase tileOnNewPosition = TileOnPosition(newPosition);
    if (tileOnNewPosition == null) {
        return;
    }
    else if (allowedTiles.Contain(tileOnNewPosition)) {
        if (tileOnNewPosition.name == "cutter") {
            // Replace the tile with a grass tile
            Vector3Int cellPosition = tilemap.WorldToCell(newPosition);
            TileBase grassTile = allowedTiles.GetTile("grass");
            tilemap.SetTile(cellPosition, grassTile);
            isCutter = true;
            allowedTiles.AddTile(SpecialTiles);
        }
        else if (tileOnNewPosition.name == "goat") {
            // Replace the tile with a grass tile
            Vector3Int cellPosition = tilemap.WorldToCell(newPosition);
            TileBase hillsTile = allowedTiles.GetTile("hills");
            tilemap.SetTile(cellPosition, hillsTile);
            allowedTiles.AddTile(SpecialTiles);
        }
        else if (tileOnNewPosition.name == "mountains" && isCutter) {
            // Replace the tile with a grass tile
            Vector3Int cellPosition = tilemap.WorldToCell(newPosition);
            TileBase grassTile = allowedTiles.GetTile("grass");
            tilemap.SetTile(cellPosition, grassTile);
        }
        else {
            // Move to the new position
            transform.position = newPosition;
        }
    }
    else {
        Debug.Log("You cannot walk on " + tileOnNewPosition.name + "!");
    }
}

}
