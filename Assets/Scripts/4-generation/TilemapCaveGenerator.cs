using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;


/**
 * This class demonstrates the CaveGenerator on a Tilemap.
 * 
 * By: Erel Segal-Halevi
 * Since: 2020-12
 */

public class TilemapCaveGenerator: MonoBehaviour {
    [SerializeField] Tilemap tilemap = null;

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField] TileBase wallTile = null;

    [Tooltip("The tile that represents a floor (a passable block)")]
    [SerializeField] TileBase floorTile = null;

    [Tooltip("The percent of walls in the initial random map")]
    [Range(0, 1)]
    [SerializeField] float randomFillPercent = 0.5f;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField] int simulationSteps = 20;

    [Tooltip("For how long will we pause between each simulation step so we can look at the result?")]
    [SerializeField] float pauseTime = 1f;

    [SerializeField] GameObject player = null;


    private CaveGenerator caveGenerator;

    void Start()  {
        //To get the same random numbers each time we run the script
        Random.InitState(100);

        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();
                
        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());
            
        //Start the simulation
        StartCoroutine(SimulateCavePattern());
    }


    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePattern()  {
        TileBase[] allowedTiles = {floorTile};
        TilemapGraph tilemapGraph = new TilemapGraph(tilemap, allowedTiles);
        for (int i = 0; i < simulationSteps; i++)   {
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            caveGenerator.SmoothMap();

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(caveGenerator.GetMap());
            int counter=0;
            Vector3Int startNode =  tilemap.WorldToCell(player.transform.position);
            for(int x=0;x<gridSize;x++){
                for(int y=0;y<gridSize;y++){
                    if(caveGenerator.GetMap()[x,y]==0){
                    Vector3Int endNode =  tilemap.WorldToCell(new Vector3Int(x, y, 0));
                    List<Vector3Int> shortestPath = BFS.GetPath(tilemapGraph, startNode, endNode, 100);
                        if(shortestPath.Count>0){
                            counter++;
                        }                    
                    }   
                }
            }
            if(counter<100 && i== simulationSteps-1){
                i=0;
            }
            else{
                break;   
            }

        }
        Debug.Log("Simulation completed!");
    }



    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data) {
        int count =0;
        for (int y = 0; y < gridSize; y++) {
            for (int x = 0; x < gridSize; x++) {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y] == 1 ? wallTile: floorTile;
                if(data[x,y]==0 && count==0){
                    count++;
                    player.transform.position = position;
                }
                tilemap.SetTile(position, tile);
            }
        }
    }
}
