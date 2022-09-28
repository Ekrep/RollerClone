using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int posOnX;
    [HideInInspector] public int posOnZ;
    [HideInInspector] public bool isBlocked;
     public bool isKeyTile;
    public List<Tile> neighbourTiles;
    [HideInInspector] public MeshRenderer tileColor;
    [HideInInspector] public Vector2Int tilePosVec2;

    public GameObject cube;

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

        tileColor = GetComponent<MeshRenderer>();
        

    }
    private void Update()
    {
        /*if (tileColor.material.color != Color.red)
        {
            if (isBlocked)
            {

                tileColor.material.color = Color.red;
            }
        }
        if (!isBlocked)
        {
            tileColor.material.color = Color.white;
        }
       /* if (isKeyTile)
        {
            tileColor.material.color = Color.black;
        }*/

        /*if (isBlocked)
        {
            cube.SetActive(true);
        }
        else
        {
            cube.SetActive(false);
        }*/
    }



    private void GetNeighbourTiles()
    {
        List<Tile> allTileListFlag = GridManager.Instance.allTiles;
        for (int i = 0; i < allTileListFlag.Count; i++)
        {
            if (allTileListFlag[i].posOnX == this.posOnX + 1 && allTileListFlag[i].posOnZ == this.posOnZ && posOnX != GridManager.Instance.gridWidth)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                rightNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnX == this.posOnX - 1 && allTileListFlag[i].posOnZ == this.posOnZ)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                leftNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ + 1 && allTileListFlag[i].posOnX == this.posOnX && posOnZ != GridManager.Instance.gridHeight)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                upNeighbour = allTileListFlag[i];
            }
            if (allTileListFlag[i].posOnZ == this.posOnZ - 1 && allTileListFlag[i].posOnX == this.posOnX)
            {
                neighbourTiles.Add(allTileListFlag[i]);
                downNeighbour = allTileListFlag[i];
            }


        }
    }
}
