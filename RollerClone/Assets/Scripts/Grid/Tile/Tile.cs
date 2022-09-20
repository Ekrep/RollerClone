using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int posOnX;
    [HideInInspector] public int posOnZ;
    [HideInInspector] public bool isBlocked;
    public List<Tile> neighbourTiles;

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



    private void GetNeighbourTiles()
    {
        List<Tile> allTileListFlag = GridManager.Instance.allTiles;
        for (int i = 0; i < allTileListFlag.Count; i++)
        {
            if (allTileListFlag[i].posOnX == this.posOnX + 10 && allTileListFlag[i].posOnZ == this.posOnZ && posOnX != GridManager.Instance.gridWidth)
            {
                neighbourTiles.Add(allTileListFlag[i]);
            }
            if (allTileListFlag[i].posOnX == this.posOnX - 10 && allTileListFlag[i].posOnZ == this.posOnZ)
            {
                neighbourTiles.Add(allTileListFlag[i]);
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ + 10 && allTileListFlag[i].posOnX == this.posOnX && posOnZ != GridManager.Instance.gridHeight)
            {
                neighbourTiles.Add(allTileListFlag[i]);
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ - 10 && allTileListFlag[i].posOnX == this.posOnX)
            {
                neighbourTiles.Add(allTileListFlag[i]);
            }


        }
    }
}
