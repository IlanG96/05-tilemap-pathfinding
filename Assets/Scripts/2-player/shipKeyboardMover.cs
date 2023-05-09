using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class shipKeyboardMover: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] GameObject player = null;
    public bool isShip = false;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

            isShip = true;
            player.transform.parent = gameObject.transform;
            player.SetActive(false); 
        }
        
    }
    

    private void Update()
    {
        if (!isShip)
        {
            player.SetActive(true);
            return;
        }

        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        // Check if the ship is on a new tile that is not in the allowed tiles
        if (tileOnNewPosition != null && !allowedTiles.Contain(tileOnNewPosition))
        {
            isShip = false;
            player.transform.parent = null;
            player.transform.position = newPosition;

            return;
        }

        if(tileOnNewPosition==null)
            return;    
        
        transform.position = newPosition;
    }


}
