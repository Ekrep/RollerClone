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


    int x = 0;


    public List<Tile> allTiles;


    public Tile selectedTile;


    public List<Tile> unblockedTiles;



    private Enums.MoveablePathCreateType _firstSelection;
    private Enums.MoveablePathCreateType _secondSelection;


    

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
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 1), Random.Range(1, gridHeight - 1))];
        GameManager.Instance.OnSendStartingTileToBall(selectedTile);
        
        

        for (int i = 0; i < 100; i++)
        {
            int pathLength = 4;
            /*if (x<3)
            {
                _pathCreateType = (Enums.MoveablePathCreateType)x;
                Debug.Log(x);
                x++;
                if (x==3)
                {
                    x = 0;
                }
            }*/
            _pathCreateType= (Enums.MoveablePathCreateType)Random.Range(0, 3);
            
            switch (_pathCreateType)
            {

                case Enums.MoveablePathCreateType.Up:

                    for (int j = 0; j < pathLength; j++)
                    {
                        if (selectedTile.upNeighbour != null&&selectedTile.upNeighbour.isBlocked)
                        {
                           
                            if (!unblockedTiles.Contains(selectedTile.upNeighbour))
                            {

                                Debug.Log(selectedTile + "selected");
                                Debug.Log(selectedTile.upNeighbour + "selectedUp");
                                selectedTile.isBlocked = false;
                                selectedTile.upNeighbour.isBlocked = false;
                                selectedTile = selectedTile.upNeighbour;
                                unblockedTiles.Add(selectedTile);

                            }
                            
                            
                            
                        }

                    }
                    break;
                case Enums.MoveablePathCreateType.Down:
                   
                    for (int j = 0; j < pathLength; j++)
                    {

                        if (selectedTile.downNeighbour != null && selectedTile.downNeighbour.isBlocked)
                        {
                            
                            if (!unblockedTiles.Contains(selectedTile.downNeighbour))
                            {
                                Debug.Log(selectedTile + "selected");
                                Debug.Log(selectedTile.upNeighbour + "selectedDown");
                                selectedTile.isBlocked = false;
                                selectedTile.downNeighbour.isBlocked = false;
                                selectedTile = selectedTile.downNeighbour;
                                unblockedTiles.Add(selectedTile);
                            }
                            
                            
                        }

                    }
                    break;
                case Enums.MoveablePathCreateType.Left:
                    
                    for (int j = 0; j < pathLength; j++)
                    {
                        if (selectedTile.leftNeighbour != null && selectedTile.leftNeighbour.isBlocked)
                        {
                            
                            if (!unblockedTiles.Contains(selectedTile.leftNeighbour))
                            {
                                Debug.Log(selectedTile + "selected");
                                Debug.Log(selectedTile.upNeighbour + "selectedLeft");
                                selectedTile.isBlocked = false;
                                selectedTile.leftNeighbour.isBlocked = false;
                                selectedTile = selectedTile.leftNeighbour;
                                unblockedTiles.Add(selectedTile);
                            }
                            
                           

                        }


                    }
                    break;
                case Enums.MoveablePathCreateType.Right:
                    
                    for (int j = 0; j< pathLength; j++)
                    {
                        if (selectedTile.rightNeighbour != null && selectedTile.rightNeighbour.isBlocked)
                        {
                            if (!unblockedTiles.Contains(selectedTile.rightNeighbour))
                            {
                                Debug.Log(selectedTile + "selected");
                                Debug.Log(selectedTile.upNeighbour + "selectedRight");
                                selectedTile.isBlocked = false;
                                selectedTile.rightNeighbour.isBlocked = false;
                                selectedTile = selectedTile.rightNeighbour;
                                unblockedTiles.Add(selectedTile);

                            }
                            
                            


                        }

                    }
                    break;
                default:
                    break;
            }

        }

       
    }



       

}
