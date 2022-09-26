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
        Debug.Log((gridWidth * 10) - 10);
        GenerateGrid();
        GenerateObstacles();
        //CreateMoveAblePath();
        CreatePath();

    }




    private void GenerateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject go;
                go = Instantiate(_tilePrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                go.GetComponent<Tile>().posOnX = x * 10;
                go.GetComponent<Tile>().posOnZ = y * 10;
                go.GetComponent<Tile>().tilePosVec2 = new Vector2Int(x, y);
                go.transform.SetParent(_tileParent);
                go.name = "Tile" + "(" + x * 10 + "," + y * 10 + ")";
                allTiles.Add(go.GetComponent<Tile>());
                tileDictionary.Add(new Vector2Int(x, y), go.GetComponent<Tile>());

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

    private void CreatePath()
    {
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        Tile firstSelectedTile = selectedTile;
        GameManager.Instance.OnSendStartPosToBall(selectedTile);

        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

        Enums.MoveablePathCreateType oldPathCreateBehaviour=_pathCreateType;

        int iterationCount = 20;
        int randomPathWayCount = 0;


        for (int i = 0; i < iterationCount; i++)
        {
            //------------------------------------------------------>
            //UP
            if (_pathCreateType == Enums.MoveablePathCreateType.Up)
            {
                randomPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                Debug.Log("up \n"+randomPathWayCount);
                selectedTile.isBlocked = false;
                selectedTile.downNeighbour.isKeyTile = true;
                for (int j = 0; j < randomPathWayCount; j++)
                {
                    if (!selectedTile.upNeighbour.isKeyTile && selectedTile.upNeighbour.posOnZ < (gridHeight * 10) - 10)
                    {
                        selectedTile.isBlocked = false;
                        selectedTile = selectedTile.upNeighbour;
                       

                    }
                    if (selectedTile.upNeighbour.isKeyTile)
                    {
                        RemoveKeyTile(selectedTile, _pathCreateType);
                    }
                }
                if (selectedTile.upNeighbour.isBlocked)
                {
                    selectedTile.upNeighbour.isKeyTile = true;
                }
                    
                
               
                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);

            }
            //------------------------------------------------------>
            //DOWN
            if (_pathCreateType == Enums.MoveablePathCreateType.Down)
            {
                randomPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                Debug.Log("down \n"+randomPathWayCount);
                selectedTile.isBlocked = false;
                selectedTile.upNeighbour.isKeyTile = true;
                for (int j = 0; j < randomPathWayCount; j++)
                {
                    if (!selectedTile.downNeighbour.isKeyTile && selectedTile.downNeighbour.posOnZ >= 10)
                    {
                        selectedTile.isBlocked = false;
                        selectedTile = selectedTile.downNeighbour;
                        

                    }
                    if (selectedTile.downNeighbour.isKeyTile)
                    {
                        RemoveKeyTile(selectedTile, _pathCreateType);
                    }
                }
                if (selectedTile.downNeighbour.isBlocked)
                {
                    selectedTile.downNeighbour.isKeyTile = true;
                }
                    
                
                
                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);

            }
            //------------------------------------------------------>
            //LEFT
            if (_pathCreateType == Enums.MoveablePathCreateType.Left)
            {
                randomPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                Debug.Log("left \n"+randomPathWayCount);
                selectedTile.isBlocked = false;
                selectedTile.rightNeighbour.isKeyTile = true;
                for (int j = 0; j < randomPathWayCount; j++)
                {
                    if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX >= 10)
                    {
                        selectedTile.isBlocked = false;
                        selectedTile = selectedTile.leftNeighbour;
                        

                    }
                    if (selectedTile.leftNeighbour.isKeyTile)
                    {
                        RemoveKeyTile(selectedTile, _pathCreateType);
                    }
                }
                if (selectedTile.leftNeighbour.isBlocked)
                {
                    selectedTile.leftNeighbour.isKeyTile = true;
                }
                    
                
                
                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

            }
            //------------------------------------------------------>
            //RIGHT
            if (_pathCreateType == Enums.MoveablePathCreateType.Right)
            {
                randomPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                Debug.Log("Right \n"+randomPathWayCount);
                selectedTile.isBlocked = false;
                selectedTile.leftNeighbour.isKeyTile = true;
                for (int j = 0; j < randomPathWayCount; j++)
                {
                    if (!selectedTile.rightNeighbour.isKeyTile && selectedTile.rightNeighbour.posOnX < (gridWidth * 10) - 10)
                    {
                        selectedTile.isBlocked = false;
                        selectedTile = selectedTile.rightNeighbour;
                        

                    }
                    if (selectedTile.rightNeighbour.isKeyTile)
                    {
                        RemoveKeyTile(selectedTile, _pathCreateType);
                    }
                    
                }
                if (selectedTile.rightNeighbour.isBlocked)
                {
                    selectedTile.rightNeighbour.isKeyTile = true;
                }
                    
                
                
                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

            }
        }
    }



    private void CreateMoveAblePath()
    {
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        Tile firstSelectedTile = selectedTile;
        GameManager.Instance.OnSendStartPosToBall(selectedTile);


        Enums.MoveablePathCreateType oldPathType = Enums.MoveablePathCreateType.Base;
        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);


        int iterationCount = 250;
        int randomYPathWayCount = 0;
        int randomXPathWayCount = 0;
        for (int i = 0; i < iterationCount; i++)
        {

            switch (_pathCreateType)
            {


                case Enums.MoveablePathCreateType.Up:
                    if (oldPathType == Enums.MoveablePathCreateType.Down)
                    {
                        break;
                    }
                    randomYPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    oldPathType = Enums.MoveablePathCreateType.Up;
                    if (selectedTile.downNeighbour.isBlocked)
                    {

                        selectedTile.downNeighbour.isKeyTile = true;

                    }

                    selectedTile.isBlocked = false;
                    for (int j = 0; j < randomYPathWayCount; j++)
                    {
                        if (!selectedTile.upNeighbour.isKeyTile && selectedTile.upNeighbour.posOnZ < (gridHeight * 10) - 10)
                        {
                            if (selectedTile.upNeighbour.isBlocked)
                            {
                                Debug.Log("Gidiyorum UP" + randomYPathWayCount);
                                selectedTile = selectedTile.upNeighbour;
                                selectedTile.isBlocked = false;
                            }
                            else
                            {
                                selectedTile = firstSelectedTile;
                                _pathCreateType = FirstPathCreateBehaviour(oldPathType);
                                break;

                            }

                        }
                        Debug.Log("KEYTILE UP");


                    }
                    if (selectedTile.upNeighbour.isBlocked)
                    {
                        selectedTile.upNeighbour.isKeyTile = true;
                    }


                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                    oldPathType = _pathCreateType;

                    // Debug.Log("0 UP");
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);







                    break;


                case Enums.MoveablePathCreateType.Down:
                    if (oldPathType == Enums.MoveablePathCreateType.Up)
                    {
                        break;
                    }
                    randomYPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    oldPathType = Enums.MoveablePathCreateType.Down;
                    selectedTile.isBlocked = false;
                    if (selectedTile.upNeighbour.isBlocked)
                    {
                        selectedTile.upNeighbour.isKeyTile = true;
                    }


                    for (int j = 0; j < randomYPathWayCount; j++)
                    {
                        if (!selectedTile.downNeighbour.isKeyTile && selectedTile.downNeighbour.posOnZ > 10)
                        {
                            if (selectedTile.downNeighbour.isBlocked)
                            {
                                Debug.Log("Gidiyorum DOWN" + randomYPathWayCount);
                                selectedTile = selectedTile.downNeighbour;
                                selectedTile.isBlocked = false;

                            }
                            else
                            {
                                selectedTile = firstSelectedTile;
                                _pathCreateType = FirstPathCreateBehaviour(oldPathType);
                                break;


                            }

                        }

                        Debug.Log("KEYTILE DOWN");

                    }
                    if (selectedTile.downNeighbour.isBlocked)
                    {
                        selectedTile.downNeighbour.isKeyTile = true;
                    }


                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                    oldPathType = _pathCreateType;

                    //Debug.Log("0 DOWN");
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);



                    break;


                case Enums.MoveablePathCreateType.Left:
                    if (oldPathType == Enums.MoveablePathCreateType.Left)
                    {
                        break;
                    }
                    randomXPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    oldPathType = Enums.MoveablePathCreateType.Left;
                    selectedTile.isBlocked = false;
                    if (selectedTile.rightNeighbour.isBlocked)
                    {
                        selectedTile.rightNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < randomXPathWayCount; j++)
                    {
                        if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX > 10)
                        {
                            if (selectedTile.leftNeighbour.isBlocked)
                            {
                                Debug.Log("Gidiyorum LEFT" + randomXPathWayCount);
                                selectedTile = selectedTile.leftNeighbour;
                                selectedTile.isBlocked = false;
                            }
                            else
                            {

                                selectedTile = firstSelectedTile;
                                _pathCreateType = FirstPathCreateBehaviour(oldPathType);
                                break;

                            }

                        }
                        Debug.Log("KEYTILE LEFT");

                    }
                    if (selectedTile.leftNeighbour.isBlocked)
                    {
                        selectedTile.leftNeighbour.isKeyTile = true;

                    }

                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                    oldPathType = _pathCreateType;

                    //Debug.Log("0 LEFT");
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);



                    break;


                case Enums.MoveablePathCreateType.Right:
                    if (oldPathType == Enums.MoveablePathCreateType.Right)
                    {
                        break;
                    }
                    randomXPathWayCount = Random.Range(1, FindRandomDir(selectedTile.tilePosVec2, _pathCreateType));
                    oldPathType = Enums.MoveablePathCreateType.Right;
                    selectedTile.isBlocked = false;
                    if (selectedTile.leftNeighbour.isBlocked)
                    {
                        selectedTile.leftNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < randomXPathWayCount; j++)
                    {
                        if (!selectedTile.rightNeighbour.isKeyTile && selectedTile.rightNeighbour.posOnX < (gridWidth * 10) - 10)
                        {
                            if (selectedTile.rightNeighbour.isBlocked)
                            {
                                Debug.Log("Gidiyorum RIGHT" + randomXPathWayCount);
                                selectedTile = selectedTile.rightNeighbour;
                                selectedTile.isBlocked = false;
                            }
                            else
                            {
                                selectedTile = firstSelectedTile;
                                _pathCreateType = FirstPathCreateBehaviour(oldPathType);
                                break;

                            }

                        }
                        Debug.Log("KEYTILE RIGHT");

                    }
                    if (selectedTile.rightNeighbour.isBlocked)
                    {
                        selectedTile.rightNeighbour.isKeyTile = true;
                    }

                    _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                    oldPathType = _pathCreateType;
                    // Debug.Log("0 RIGHT");




                    break;
                default:
                    Debug.Log("DEFAULT");
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
        int maxLength = 0;
        if (dir == Enums.MoveablePathCreateType.Up)
        {
            maxLength = gridHeight - selectedTile.tilePosVec2.y - 1;
        }
        if (dir == Enums.MoveablePathCreateType.Down)
        {
            maxLength = selectedTile.tilePosVec2.y - 1;
        }
        if (dir == Enums.MoveablePathCreateType.Right)
        {
            maxLength = gridWidth - selectedTile.tilePosVec2.x - 1;
        }
        if (dir == Enums.MoveablePathCreateType.Left)
        {
            maxLength = selectedTile.tilePosVec2.x - 1;
        }

        return maxLength;
    }


    private Enums.MoveablePathCreateType FirstPathCreateBehaviour(Enums.MoveablePathCreateType type)
    {
        if (type == Enums.MoveablePathCreateType.Up || type == Enums.MoveablePathCreateType.Down)
        {
            return (Enums.MoveablePathCreateType)Random.Range(2, 4);
        }
        if (type == Enums.MoveablePathCreateType.Right || type == Enums.MoveablePathCreateType.Left)
        {
            return (Enums.MoveablePathCreateType)Random.Range(0, 2);
        }


        return 0;
    }

    private void RemoveKeyTile(Tile currentTile,Enums.MoveablePathCreateType behaviour)
    {
        if (behaviour==Enums.MoveablePathCreateType.Up&&currentTile.upNeighbour.isKeyTile&&currentTile.rightNeighbour.isKeyTile&&currentTile.leftNeighbour.isKeyTile)
        {
            currentTile.upNeighbour.isKeyTile = false;
        }
        if (behaviour == Enums.MoveablePathCreateType.Down && currentTile.downNeighbour.isKeyTile && currentTile.rightNeighbour.isKeyTile && currentTile.leftNeighbour.isKeyTile)
        {
            currentTile.downNeighbour.isKeyTile = false;
        }
        if (behaviour == Enums.MoveablePathCreateType.Left && currentTile.leftNeighbour.isKeyTile && currentTile.upNeighbour.isKeyTile && currentTile.downNeighbour.isKeyTile)
        {
            currentTile.leftNeighbour.isKeyTile = false;
        }
        if (behaviour == Enums.MoveablePathCreateType.Left && currentTile.rightNeighbour.isKeyTile && currentTile.upNeighbour.isKeyTile && currentTile.downNeighbour.isKeyTile)
        {
            currentTile.rightNeighbour.isKeyTile = false;
        }
    }





}
