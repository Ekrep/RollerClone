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
        //CreatePath();
        CreateRealPathThisTime();

    }




    private void GenerateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject go;
                go = Instantiate(_tilePrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                go.GetComponent<Tile>().posOnX = x;
                go.GetComponent<Tile>().posOnZ = y;
                go.GetComponent<Tile>().tilePosVec2 = new Vector2Int(x, y);
                go.transform.SetParent(_tileParent);
                go.name = "Tile" + "(" + x + "," + y + ")";
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

    private void CreateRealPathThisTime()
    {
        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        GameManager.Instance.OnSendStartPosToBall(selectedTile);
        Tile firstSelectedTile = selectedTile;

        Enums.MoveablePathCreateType oldPathCreateType = Enums.MoveablePathCreateType.Base;

        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);



        //gitti�i y�n�n tersine gitmeme ekle
        int iterationCount = 50;
        int pathWayIterationCount;
        for (int i = 0; i < iterationCount; i++)
        {
            switch (_pathCreateType)
            {
                case Enums.MoveablePathCreateType.Up:
                    Debug.Log("----------TRY'N UP--------------");
                    if (!selectedTile.upNeighbour.isKeyTile)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        selectedTile.isBlocked = false;
                        if (selectedTile.downNeighbour.isBlocked)
                        {
                            selectedTile.downNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Up" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!selectedTile.upNeighbour.isKeyTile && selectedTile.upNeighbour.posOnZ != gridHeight - 1)
                            {
                                //if delete this "if" makes more branch but bugged!
                                
                                    selectedTile = selectedTile.upNeighbour;
                                    selectedTile.isBlocked = false;
                                    visitedTiles.Add(selectedTile);
                                
                               
                               


                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("UPBREAK");
                                //holder.downNeighbour.isKeyTile = false;
                                if (selectedTile.downNeighbour.isBlocked)
                                {
                                    selectedTile.upNeighbour.isKeyTile = true;
                                }
                                if (selectedTile.upNeighbour.posOnZ == gridHeight - 1 && visitedTiles.Count < 2 && selectedTile.downNeighbour.isBlocked)
                                {
                                    selectedTile.downNeighbour.isKeyTile = false;
                                    _pathCreateType = Enums.MoveablePathCreateType.Down;
                                }
                                else
                                {
                                    if (selectedTile.leftNeighbour.isBlocked)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;
                                    }
                                    else
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Right;
                                    }
                                    if (!selectedTile.leftNeighbour.isBlocked&&!selectedTile.rightNeighbour.isBlocked)
                                    {
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                    }
                                   // _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                                }

                                break;
                            }

                        }
                        if (selectedTile.upNeighbour.isBlocked)
                        {
                            selectedTile.upNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (selectedTile.upNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                            }
                            else
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                            }
                            
                        }

                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Left;
                    }


                    break;
                case Enums.MoveablePathCreateType.Down:
                    Debug.Log("----------TRY'N DOWN--------------");
                    if (!selectedTile.downNeighbour.isKeyTile)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        selectedTile.isBlocked = false;
                        if (selectedTile.upNeighbour.isBlocked)
                        {
                            selectedTile.upNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Down" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!selectedTile.downNeighbour.isKeyTile && selectedTile.downNeighbour.posOnZ >= 1)
                            {
                                   selectedTile = selectedTile.downNeighbour;
                                    selectedTile.isBlocked = false;
                                    visitedTiles.Add(selectedTile);
                                

                                
                               

                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("DOWNBREAK");
                                //holder.upNeighbour.isKeyTile = false;
                                if (selectedTile.downNeighbour.isBlocked)
                                {
                                    selectedTile.downNeighbour.isKeyTile = true;
                                }
                                if (selectedTile.downNeighbour.posOnZ == 0 && visitedTiles.Count < 2 && selectedTile.upNeighbour.isBlocked)
                                {
                                    selectedTile.upNeighbour.isKeyTile = false;
                                    _pathCreateType = Enums.MoveablePathCreateType.Up;
                                }
                                else
                                {
                                    if (selectedTile.leftNeighbour.isBlocked)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;
                                    }
                                    else
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Right;
                                    }
                                    if (!selectedTile.leftNeighbour.isBlocked && !selectedTile.rightNeighbour.isBlocked)
                                    {
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                    }

                                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                                }

                                break;
                            }

                        }
                        if (selectedTile.downNeighbour.isBlocked)
                        {
                            selectedTile.downNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (selectedTile.downNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                            }
                            else
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                            }
                        }

                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Right;
                    }


                    break;
                case Enums.MoveablePathCreateType.Left:
                    Debug.Log("----------TRY'N LEFT--------------");
                    if (!selectedTile.leftNeighbour.isKeyTile)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        holder = selectedTile;
                        bool isBreak = false;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        selectedTile.isBlocked = false;
                        if (selectedTile.rightNeighbour.isBlocked)
                        {
                            selectedTile.rightNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Left" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX >= 1)
                            {
                                    selectedTile = selectedTile.leftNeighbour;
                                    selectedTile.isBlocked = false;
                                    visitedTiles.Add(selectedTile);
                               
                               
                               


                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("LEFTBREAK");
                                //holder.rightNeighbour.isKeyTile = false;
                                if (selectedTile.leftNeighbour.isBlocked)
                                {
                                    selectedTile.leftNeighbour.isKeyTile = true;
                                }
                                if (selectedTile.leftNeighbour.posOnX == 0 && visitedTiles.Count < 2 && selectedTile.rightNeighbour.isBlocked)
                                {
                                    selectedTile.rightNeighbour.isKeyTile = false;
                                    _pathCreateType = Enums.MoveablePathCreateType.Right;
                                }
                                else
                                {
                                    if (selectedTile.upNeighbour.isBlocked)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;
                                    }
                                    else
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Down;
                                    }
                                    if (!selectedTile.upNeighbour.isBlocked && !selectedTile.downNeighbour.isBlocked)
                                    {
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                    }
                                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                                }

                                break;
                            }

                        }
                        if (selectedTile.leftNeighbour.isBlocked)
                        {
                            selectedTile.leftNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (selectedTile.leftNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                            }
                            else
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                            }
                            
                        }


                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Down;
                    }


                    break;
                case Enums.MoveablePathCreateType.Right:
                    Debug.Log("----------TRY'N RIGHT--------------");
                    if (!selectedTile.rightNeighbour.isKeyTile)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        selectedTile.isBlocked = false;
                        if (selectedTile.leftNeighbour.isBlocked)
                        {
                            selectedTile.leftNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Right" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!selectedTile.rightNeighbour.isKeyTile && selectedTile.rightNeighbour.posOnX != gridWidth - 1)
                            {
                                
                                    selectedTile = selectedTile.rightNeighbour;
                                    selectedTile.isBlocked = false;
                                    visitedTiles.Add(selectedTile);
                                
                                
                               


                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("RIGHTBREAK");
                                //holder.leftNeighbour.isKeyTile = false;
                                if (selectedTile.rightNeighbour.isBlocked)
                                {
                                    selectedTile.rightNeighbour.isKeyTile = true;
                                }
                                if (selectedTile.rightNeighbour.posOnX == gridWidth - 1 && visitedTiles.Count < 2 && selectedTile.leftNeighbour.isBlocked)
                                {

                                    selectedTile.leftNeighbour.isKeyTile = false;
                                    _pathCreateType = Enums.MoveablePathCreateType.Left;
                                }
                                else
                                {
                                    if (selectedTile.upNeighbour.isBlocked)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;
                                    }
                                    else
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Down;
                                    }
                                    if (!selectedTile.upNeighbour.isBlocked && !selectedTile.downNeighbour.isBlocked)
                                    {
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                    }
                                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                                }

                                break;

                            }
                        }
                        if (selectedTile.rightNeighbour.isBlocked)
                        {
                            selectedTile.rightNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (selectedTile.leftNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);
                            }
                            else
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);
                            }
                        }

                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Up;
                    }


                    break;
                case Enums.MoveablePathCreateType.Base:
                    break;
                default:
                    break;
            }
        }





        //GArbage
        /*for (int i = 0; i < iterationCount; i++)
        {
            switch (_pathCreateType)
            {
                case Enums.MoveablePathCreateType.Up:
                    //Debug.Log(oldPathCreateType + "oldpathway");

                    //Debug.Log("Denedim U");
                    pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                    selectedTile.isBlocked = false;
                    if (selectedTile.downNeighbour.isBlocked)
                    {
                        selectedTile.downNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < pathWayIterationCount; j++)
                    {
                        if (!selectedTile.upNeighbour.isKeyTile && selectedTile.upNeighbour.posOnZ != gridHeight - 1)
                        {
                            Debug.Log("up" + pathWayIterationCount);
                            selectedTile = selectedTile.upNeighbour;
                            selectedTile.isBlocked = false;
                            visitedTiles.Add(selectedTile);
                            oldPathCreateType = _pathCreateType;
                        }


                    }
                    if (selectedTile.upNeighbour.isBlocked)
                    {
                        selectedTile.upNeighbour.isKeyTile = true;
                    }
                    if (selectedTile.leftNeighbour.posOnX <= 1)
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Right;
                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Left;
                    }
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);



                    break;


                case Enums.MoveablePathCreateType.Down:
                    //Debug.Log(oldPathCreateType + "oldpathway");

                    //Debug.Log("Denedim D");
                    pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                    selectedTile.isBlocked = false;
                    if (selectedTile.upNeighbour.isBlocked)
                    {
                        selectedTile.upNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < pathWayIterationCount; j++)
                    {
                        if (!selectedTile.downNeighbour.isKeyTile && selectedTile.downNeighbour.posOnZ >= 1)
                        {
                            Debug.Log("down" + pathWayIterationCount);
                            selectedTile = selectedTile.downNeighbour;
                            selectedTile.isBlocked = false;
                            visitedTiles.Add(selectedTile);
                            oldPathCreateType = _pathCreateType;
                        }


                    }
                    if (selectedTile.downNeighbour.isBlocked)
                    {
                        selectedTile.upNeighbour.isKeyTile = true;
                    }
                    if (selectedTile.leftNeighbour.posOnX <= 1)
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Right;
                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Left;
                    }
                    // _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);



                    break;


                case Enums.MoveablePathCreateType.Left:
                    //Debug.Log(oldPathCreateType + "oldpathway");

                    //Debug.Log("Denedim L");
                    pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                    selectedTile.isBlocked = false;
                    if (selectedTile.rightNeighbour.isBlocked)
                    {
                        selectedTile.rightNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < pathWayIterationCount; j++)
                    {
                        if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX >= 1)
                        {
                            Debug.Log("left" + pathWayIterationCount);
                            selectedTile = selectedTile.leftNeighbour;
                            selectedTile.isBlocked = false;
                            visitedTiles.Add(selectedTile);
                            oldPathCreateType = _pathCreateType;
                        }


                    }
                    if (selectedTile.leftNeighbour.isBlocked)
                    {
                        selectedTile.leftNeighbour.isKeyTile = true;
                    }
                    if (selectedTile.upNeighbour.posOnZ >= gridHeight - 2)
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Down;
                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Up;
                    }
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);



                    break;


                case Enums.MoveablePathCreateType.Right:
                    //Debug.Log(oldPathCreateType+"oldpathway");

                    //Debug.Log("Denedim R");
                    pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                    selectedTile.isBlocked = false;
                    if (selectedTile.leftNeighbour.isBlocked)
                    {
                        selectedTile.leftNeighbour.isKeyTile = true;
                    }

                    for (int j = 0; j < pathWayIterationCount; j++)
                    {
                        if (!selectedTile.rightNeighbour.isKeyTile && selectedTile.rightNeighbour.posOnX != gridWidth - 1)
                        {
                            Debug.Log("right" + pathWayIterationCount);
                            selectedTile = selectedTile.rightNeighbour;
                            selectedTile.isBlocked = false;
                            visitedTiles.Add(selectedTile);
                            oldPathCreateType = _pathCreateType;
                        }


                    }
                    if (selectedTile.rightNeighbour.isBlocked)
                    {
                        selectedTile.rightNeighbour.isKeyTile = true;
                    }
                    if (selectedTile.upNeighbour.posOnZ >= gridHeight - 2)
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Down;
                    }
                    else
                    {
                        _pathCreateType = Enums.MoveablePathCreateType.Up;
                    }
                    //_pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);


                    break;
                case Enums.MoveablePathCreateType.Base:
                    break;
                default:
                    break;
            }
        }*/
    }


    private int FindRandomDir(Tile selectedTile, Enums.MoveablePathCreateType dir)
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

    private void RemoveKeyTile(Tile currentTile, Enums.MoveablePathCreateType behaviour)
    {
        if (behaviour == Enums.MoveablePathCreateType.Up && currentTile.upNeighbour.isKeyTile && currentTile.rightNeighbour.isKeyTile && currentTile.leftNeighbour.isKeyTile)
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
