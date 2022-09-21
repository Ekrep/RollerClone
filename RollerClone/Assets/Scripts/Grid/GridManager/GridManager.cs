using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;


    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _obstaclePrefab;
    public int gridHeight;
    public int gridWidth;
    [SerializeField] private Transform _tileParent;

    [SerializeField]private Enums.ObstacleType _obstacleType;

    public List<Tile> allTiles;





    Tile selectedTile;
    Tile secondTile;



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        GenerateObstacles();

    }




    private void GenerateGrid()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject go;
                go = Instantiate(_tilePrefab, new Vector3(i * 10, 0, j * 10), Quaternion.identity);
                go.GetComponent<Tile>().posOnX = i * 10;
                go.GetComponent<Tile>().posOnZ = j * 10;
                go.transform.SetParent(_tileParent);
                go.name = "Tile" + "(" + i * 10 + "," + j * 10 + ")";
                allTiles.Add(go.GetComponent<Tile>());
            }
        }
        GameManager.Instance.OnTiled();
    }

    private void GenerateObstacles()
    {
        
        //Tile 



        switch (_obstacleType)
        {
            case Enums.ObstacleType.DotObstacle:
                selectedTile = allTiles[Random.Range(0, allTiles.Count)];
                selectedTile.isBlocked = true;
                break;
            case Enums.ObstacleType.LObstacle:
                selectedTile = allTiles[Random.Range(10, allTiles.Count - 10)];
                for (int i = 0; i < 4; i++)
                {
                    selectedTile = selectedTile.downNeighbour;
                    selectedTile.isBlocked = true;
                    
                }
                secondTile = selectedTile.rightNeighbour;
                secondTile.isBlocked = true;
                


                break;
            case Enums.ObstacleType.ReverseLObstacle:
                break;
            case Enums.ObstacleType.HorizontalIObstacle:
                break;
            case Enums.ObstacleType.VerticalIObstacle:
                break;
            case Enums.ObstacleType.OpenSquareObstacle:
                break;
            case Enums.ObstacleType.UObstacle:
                break;
            case Enums.ObstacleType.ReverseUObstacle:
                break;
            default:
                break;
        }


    }


}
