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

    private Enums.MoveablePathCreateType _pathCreateType;



    public Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>();





    public List<Tile> allTiles;


    public Tile selectedTile;









    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        GenerateObstacles();
        CreateMoveAblePath();

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
                tileDictionary.Add(new Vector2Int(i, j), go.GetComponent<Tile>());
                if (i % 2 == 0 && j % 2 == 0)
                {
                    go.GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else
                {
                    go.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
        GameManager.Instance.OnTiled();
    }


    //Needs Adjustments!!
    private void GenerateObstacles()
    {

        for (int i = 0; i < allTiles.Count; i++)
        {
            allTiles[i].isBlocked = true;
        }
       


    }

    private void CreateMoveAblePath()
    {
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth-1), Random.Range(1, gridHeight-1))];
        GameManager.Instance.OnSendStartPosToBall(selectedTile);    
        
        

        for (int i = 0; i < 200; i++)
        {
            int multiplier = Random.Range(1,3);
            _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);
            switch (_pathCreateType)
            {

                case Enums.MoveablePathCreateType.Up:

                    for (int j = 0; j < multiplier; j++)
                    {
                        if (selectedTile.upNeighbour != null&&selectedTile.upNeighbour.isBlocked&&selectedTile.upNeighbour.posOnZ<(gridHeight-1)*10
                            &&selectedTile.upNeighbour.posOnZ>10)
                        {
                            
                                Debug.Log("Switch up");
                                selectedTile.isBlocked = false;
                                selectedTile.upNeighbour.isBlocked = false;
                                selectedTile = selectedTile.upNeighbour;
                            
                            
                        }

                    }
                    break;
                case Enums.MoveablePathCreateType.Down:
                    Debug.Log("Switch down");
                    for (int j = 0; j < multiplier; j++)
                    {
                        if (selectedTile.downNeighbour != null && selectedTile.downNeighbour.isBlocked&&selectedTile.downNeighbour.posOnZ>10
                            &&selectedTile.downNeighbour.posOnZ<(gridHeight-1)*10)
                        {
                            
                                Debug.Log("Switch up");
                                selectedTile.isBlocked = false;
                                selectedTile.downNeighbour.isBlocked = false;
                                selectedTile = selectedTile.downNeighbour;
                            
                            
                        }

                    }
                    break;
                case Enums.MoveablePathCreateType.Left:
                    Debug.Log("Switch left");
                    for (int j = 0; j < multiplier; j++)
                    {
                        if (selectedTile.leftNeighbour != null && selectedTile.leftNeighbour.isBlocked&&selectedTile.leftNeighbour.posOnX>10
                            &&selectedTile.leftNeighbour.posOnX<(gridWidth-1)*10)
                        {
                            
                                Debug.Log("Switch up");
                                selectedTile.isBlocked = false;
                                selectedTile.leftNeighbour.isBlocked = false;
                                selectedTile = selectedTile.leftNeighbour;
                            
                           
                        }


                    }
                    break;
                case Enums.MoveablePathCreateType.Right:
                    Debug.Log("Switch right");
                    for (int j = 0; j< multiplier; j++)
                    {
                        if (selectedTile.rightNeighbour != null && selectedTile.rightNeighbour.isBlocked&&selectedTile.rightNeighbour.posOnX<(gridWidth-1)*10
                            &&selectedTile.posOnX>10)
                        {
                            
                                Debug.Log("Switch up");
                                selectedTile.isBlocked = false;
                                selectedTile.rightNeighbour.isBlocked = false;
                                selectedTile = selectedTile.rightNeighbour;
                            
                            
                        }

                    }
                    break;
                default:
                    break;
            }

        }
    }



       

}
