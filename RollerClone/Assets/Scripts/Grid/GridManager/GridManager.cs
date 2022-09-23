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



    public List<Tile> unblockedTiles;

    public List<Tile> allTiles;


    public Tile selectedTile;

    public List<Tile> keyTiles;







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
                go.GetComponent<Tile>().tilePosVec2 = new Vector2Int(i, j);
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
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        GameManager.Instance.OnSendStartPosToBall(selectedTile);

        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

        int iterationCount = 250;
        int randomYPathWayCount = 0;
        int randomXPathWayCount = 0;
        for (int i = 0; i < iterationCount; i++)
        {
            
            switch (_pathCreateType)
            {
                case Enums.MoveablePathCreateType.Up:
                   
                     randomYPathWayCount = Random.Range (1,FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    selectedTile.downNeighbour.isKeyTile = true;
                    selectedTile.isBlocked = false;
                    for (int j = 0; j < randomYPathWayCount; j++)
                    {
                        
             
                        selectedTile = selectedTile.upNeighbour;
                        selectedTile.isBlocked = false;

                    }
                
                    selectedTile.upNeighbour.isKeyTile = true;
                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);




                    break;


                case Enums.MoveablePathCreateType.Down:
                   
                    randomYPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    selectedTile.isBlocked = false;
                    selectedTile.upNeighbour.isKeyTile = true;
                    for (int j = 0; j < randomYPathWayCount; j++)
                    {
                        selectedTile = selectedTile.downNeighbour;
                        selectedTile.isBlocked = false;
                        
                        
                    }
                   
                    selectedTile.downNeighbour.isKeyTile = true;
                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                    break;


                case Enums.MoveablePathCreateType.Left:
                    
                    randomXPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    selectedTile.isBlocked = false;
                    selectedTile.rightNeighbour.isKeyTile = true;
                    for (int j = 0; j < randomXPathWayCount; j++)
                    {
                        selectedTile = selectedTile.leftNeighbour;
                        selectedTile.isBlocked = false;
                        
                    }
                   
                    selectedTile.leftNeighbour.isKeyTile = true;
                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                    break;


                case Enums.MoveablePathCreateType.Right:
                   
                    randomXPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    selectedTile.isBlocked = false;
                    selectedTile.leftNeighbour.isKeyTile = true;
                    for (int j = 0; j < randomXPathWayCount; j++)
                    {
                        selectedTile = selectedTile.rightNeighbour;
                        selectedTile.isBlocked = false;
                        
                    }
                    
                    selectedTile.rightNeighbour.isKeyTile = true;
                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                    break;
                default:
                    break;
            }

        }

        


        #region garbage
        /* for (int i = 0; i < 200; i++)
         {
             int multiplier = Random.Range(1, 10);





             for (int j = 0; j < multiplier; j++)
             {
                 if (_pathCreateType == Enums.MoveablePathCreateType.Up && selectedTile.upNeighbour != null && selectedTile.upNeighbour.isBlocked && selectedTile.upNeighbour.posOnZ < (gridHeight - 1) * 10
                     && selectedTile.upNeighbour.posOnZ > 10 )
                 {
                     if (!selectedTile.isKeyTile)
                     {

                         List<Tile> keyHolder = new List<Tile>();
                         Debug.Log("Switch up");
                         selectedTile.downNeighbour.isKeyTile = true;
                         selectedTile.isBlocked = false;
                         selectedTile.upNeighbour.isBlocked = false;
                         selectedTile = selectedTile.upNeighbour;
                         unblockedTiles.Add(selectedTile);
                         keyHolder.Add(selectedTile);
                         keyHolder.Add(selectedTile.upNeighbour);
                         _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                     }

                     if (j == multiplier-1&&!selectedTile.upNeighbour.isKeyTile)
                     {
                         selectedTile.upNeighbour.isKeyTile = true;
                         keyTiles.Add(selectedTile.upNeighbour);

                     }





                 }


             }

             Debug.Log("Switch down");
             for (int j = 0; j < multiplier; j++)
             {
                 if (_pathCreateType == Enums.MoveablePathCreateType.Down && selectedTile.downNeighbour != null && selectedTile.downNeighbour.isBlocked && selectedTile.downNeighbour.posOnZ > 10
                     && selectedTile.downNeighbour.posOnZ < (gridHeight - 1) * 10 )
                 {
                     if (!selectedTile.isKeyTile)
                     {
                         List<Tile> keyHolder = new List<Tile>();
                         Debug.Log("Switch up");
                         selectedTile.upNeighbour.isKeyTile = true;
                         selectedTile.isBlocked = false;
                         selectedTile.downNeighbour.isBlocked = false;
                         selectedTile = selectedTile.downNeighbour;
                         unblockedTiles.Add(selectedTile);
                         keyHolder.Add(selectedTile);
                         keyHolder.Add(selectedTile.downNeighbour);
                         _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);

                     }
                     if (j == multiplier-1&&!selectedTile.downNeighbour.isKeyTile)
                     {
                         selectedTile.downNeighbour.isKeyTile = true;
                         keyTiles.Add(selectedTile.downNeighbour);
                     }





                 }

             }

             Debug.Log("Switch left");
             for (int j = 0; j < multiplier; j++)
             {
                 if (_pathCreateType == Enums.MoveablePathCreateType.Left && selectedTile.leftNeighbour != null && selectedTile.leftNeighbour.isBlocked && selectedTile.leftNeighbour.posOnX > 10
                     && selectedTile.leftNeighbour.posOnX < (gridWidth - 1) * 10 )
                 {
                     if (!selectedTile.isKeyTile)
                     {
                         List<Tile> keyHolder = new List<Tile>();
                         Debug.Log("Switch up");
                         selectedTile.rightNeighbour.isKeyTile = true;
                         selectedTile.isBlocked = false;
                         selectedTile.leftNeighbour.isBlocked = false;
                         selectedTile = selectedTile.leftNeighbour;
                         unblockedTiles.Add(selectedTile);
                         keyHolder.Add(selectedTile);
                         keyHolder.Add(selectedTile.leftNeighbour);
                         _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

                     }

                     if (j == multiplier-1&&!selectedTile.leftNeighbour.isKeyTile)
                     {
                         selectedTile.leftNeighbour.isKeyTile = true;
                         keyTiles.Add(selectedTile.leftNeighbour);
                     }




                 }


             }

             Debug.Log("Switch right");
             for (int j = 0; j < multiplier; j++)
             {
                 if (_pathCreateType == Enums.MoveablePathCreateType.Right && selectedTile.rightNeighbour != null && selectedTile.rightNeighbour.isBlocked && selectedTile.rightNeighbour.posOnX < (gridWidth - 1) * 10
                     && selectedTile.posOnX > 10)
                 {
                     if (!selectedTile.isKeyTile)
                     {
                         List<Tile> keyHolder = new List<Tile>();
                          Debug.Log("Switch up");
                         selectedTile.leftNeighbour.isKeyTile = true;
                         selectedTile.isBlocked = false;
                         selectedTile.rightNeighbour.isBlocked = false;
                         selectedTile = selectedTile.rightNeighbour;
                         unblockedTiles.Add(selectedTile);
                         keyHolder.Add(selectedTile);
                         keyHolder.Add(selectedTile.rightNeighbour);
                         _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);


                     }

                     if (j == multiplier-1&&!selectedTile.rightNeighbour.isKeyTile)
                     {
                         selectedTile.rightNeighbour.isKeyTile = true;
                         keyTiles.Add(selectedTile.rightNeighbour);

                     }



                 }

             }

         }*/
        #endregion




    }

    private int FindRandomDir(Vector2Int coords, Enums.MoveablePathCreateType dir)
    {
        int maxLength=0;
        if (dir == Enums.MoveablePathCreateType.Up)
        {
            maxLength = gridHeight - selectedTile.tilePosVec2.y-1;
        }
        if (dir==Enums.MoveablePathCreateType.Down)
        {
            maxLength =  selectedTile.tilePosVec2.y-1;
        }
        if (dir == Enums.MoveablePathCreateType.Right)
        {
            maxLength = gridWidth - selectedTile.tilePosVec2.x-1;
        }
        if (dir == Enums.MoveablePathCreateType.Left)
        {
            maxLength = selectedTile.tilePosVec2.x-1;
        }

        return maxLength;
    }

   








}
