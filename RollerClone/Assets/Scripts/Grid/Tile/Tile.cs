using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int posOnX;
    [HideInInspector] public int posOnZ;
    [HideInInspector] public bool isBlocked;
    public List<Tile> neighbourTiles;


    public Tile upNeighbour;
    public Tile downNeighbour;
    public Tile leftNeighbour;
    public Tile rightNeighbour;

    [HideInInspector] public Enums.TileType tileType;

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
    }

    private void GameManager_Tiled()
    {
        GetNeighbourTiles();
    }
    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
    }

    private void Start()
    {
        isBlocked = false;
    }



    private void GetNeighbourTiles()
    {
        List<Tile> allTileListFlag = GridManager.Instance.allTiles;
        for (int i = 0; i < allTileListFlag.Count; i++)
        {
            if (allTileListFlag[i].posOnX == this.posOnX + 10 && allTileListFlag[i].posOnZ == this.posOnZ && posOnX != GridManager.Instance.gridWidth)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                rightNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnX == this.posOnX - 10 && allTileListFlag[i].posOnZ == this.posOnZ)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                leftNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ + 10 && allTileListFlag[i].posOnX == this.posOnX && posOnZ != GridManager.Instance.gridHeight)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                upNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ - 10 && allTileListFlag[i].posOnX == this.posOnX)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                downNeighbour = allTileListFlag[i];
            }


        }
    }
}
