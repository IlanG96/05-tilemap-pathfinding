using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class GoatKeyboardMover: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    public bool ClimbMountain = false;
    [SerializeField] GameObject player = null;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            ClimbMountain = true;
            col.gameObject.SetActive(false); // Deactivate the player gameObject
            col.gameObject.transform.parent = gameObject.transform;
        }
    }

    void Update()
    {
        if (!ClimbMountain)
            return;


        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        // Check if the goat is on a new tile that is not in the allowed tiles
        if (tileOnNewPosition != null && !allowedTiles.Contain(tileOnNewPosition))
        {
            ClimbMountain = false;
            player.gameObject.SetActive(true); // Reactivate the player gameObject
            Vector3Int lastPosition = tilemap.WorldToCell(transform.position);
            player.transform.position = newPosition; // Set the player position to the last allowed tile the goat was on
            return;
        }

        // Check if the player is far from the goat
        float distanceToGoat = Vector3.Distance(transform.position, gameObject.transform.position);
        if (distanceToGoat > 1f)
        {
            ClimbMountain = false; // Stop following the goat
            player.gameObject.SetActive(true); // Reactivate the player gameObject
            return;
        }

        transform.position = newPosition;
    }


}
