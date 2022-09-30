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
        CheckUnblockedTiles();

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
            allTiles[i].cube.SetActive(true);
            //allTiles[i].tileColor.material.color = Color.red;
        }



    }

    private void CheckUnblockedTiles()
    {

        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].isBlocked == false)
            {
                allTiles[i].cube.SetActive(false);
                //allTiles[i].tileColor.material.color = Color.white;
            }
        }
    }


    private void CreateRealPathThisTime()
    {

        selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        Debug.Log("/////////FIRST SELECTED TILE//////////" + selectedTile);
        GameManager.Instance.OnSendStartPosToBall(selectedTile);
        Tile firstSelectedTile = selectedTile;



        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

        Enums.MoveablePathCreateType oldPathCreateType = Enums.MoveablePathCreateType.Base;

        List<Tile> unblockedTiles = new List<Tile>();


        //gittiði yönün tersine gitmeme ekle
        int iterationCount = 100;
        int pathWayIterationCount;
        for (int i = 0; i < iterationCount; i++)
        {
            switch (_pathCreateType)
            {
                case Enums.MoveablePathCreateType.Up:
                    Debug.Log("----------TRY'N UP--------------");

                    if (!selectedTile.upNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Down)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        if (!selectedTile.upNeighbour.isKeyTile && selectedTile.upNeighbour.posOnZ != gridHeight - 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

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

                                Debug.Log("Up" + " " + "gittim" + (j + 1));
                                selectedTile = selectedTile.upNeighbour;
                                selectedTile.isBlocked = false;
                                visitedTiles.Add(selectedTile);
                                if (selectedTile.upNeighbour.isBlocked == false)
                                {
                                    while (!selectedTile.upNeighbour.isBlocked)
                                    {
                                        selectedTile = selectedTile.upNeighbour;
                                        visitedTiles.Add(selectedTile);
                                        selectedTile.isBlocked = false;
                                    }
                                }







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
                                    if (selectedTile.downNeighbour.leftNeighbour.isBlocked || selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                    {
                                        if (selectedTile.downNeighbour.leftNeighbour.isBlocked && !selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                        {
                                            selectedTile.downNeighbour.leftNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            selectedTile.downNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        if (selectedTile.downNeighbour.leftNeighbour.isBlocked && selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                        {
                                            selectedTile.downNeighbour.leftNeighbour.isKeyTile = true;
                                            selectedTile.downNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        selectedTile.downNeighbour.isKeyTile = false;

                                    }

                                    _pathCreateType = Enums.MoveablePathCreateType.Down;

                                }
                                else
                                {
                                    if (selectedTile.leftNeighbour.isBlocked && !selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;

                                    }
                                    else if (!selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Right;

                                    }
                                    /*if (!selectedTile.leftNeighbour.isBlocked && !selectedTile.rightNeighbour.isBlocked)
                                    {
                                        Debug.Log("-------------SEND FIRST TILE---------------");
                                        
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);
                                        while (_pathCreateType == oldPathCreateType)
                                        {
                                            Debug.Log("girdim while");
                                            _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                        }

                                    }*/
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
                            else if (!selectedTile.upNeighbour.isKeyTile)
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

                    if (!selectedTile.downNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Up)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        if (!selectedTile.downNeighbour.isKeyTile && selectedTile.downNeighbour.posOnZ >= 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

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
                                Debug.Log("Down" + " " + "gittim" + (j + 1));
                                selectedTile = selectedTile.downNeighbour;
                                selectedTile.isBlocked = false;
                                visitedTiles.Add(selectedTile);
                                if (selectedTile.downNeighbour.isBlocked == false)
                                {
                                    while (!selectedTile.downNeighbour.isBlocked)
                                    {
                                        selectedTile = selectedTile.downNeighbour;
                                        visitedTiles.Add(selectedTile);
                                        selectedTile.isBlocked = false;
                                    }
                                }






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
                                    if (selectedTile.upNeighbour.leftNeighbour.isBlocked || selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                    {
                                        if (selectedTile.upNeighbour.leftNeighbour.isBlocked && !selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                        {
                                            selectedTile.upNeighbour.leftNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            selectedTile.upNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        if (selectedTile.upNeighbour.leftNeighbour.isBlocked && selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                        {
                                            selectedTile.upNeighbour.leftNeighbour.isKeyTile = true;
                                            selectedTile.upNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        selectedTile.upNeighbour.isKeyTile = false;

                                    }
                                    _pathCreateType = Enums.MoveablePathCreateType.Up;

                                }
                                else
                                {
                                    if (selectedTile.leftNeighbour.isBlocked && !selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;

                                    }
                                    else if (!selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Right;

                                    }
                                    /*if (!selectedTile.leftNeighbour.isBlocked && !selectedTile.rightNeighbour.isBlocked)
                                    {
                                        Debug.Log("-------------SEND FIRST TILE---------------");
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);
                                        while (_pathCreateType == oldPathCreateType)
                                        {
                                            Debug.Log("girdim while");
                                            _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                        }

                                    }*/

                                    //that part brokes
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
                            else if (!selectedTile.downNeighbour.isKeyTile)
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

                    if (!selectedTile.leftNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Right)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        holder = selectedTile;
                        bool isBreak = false;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        selectedTile.isBlocked = false;
                        if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX >= 1)
                        {
                            oldPathCreateType = _pathCreateType;

                        }

                        if (selectedTile.rightNeighbour.isBlocked)
                        {
                            selectedTile.rightNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Left" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!selectedTile.leftNeighbour.isKeyTile && selectedTile.leftNeighbour.posOnX >= 1)
                            {
                                Debug.Log("Left" + " " + "gittim" + (j + 1));
                                selectedTile = selectedTile.leftNeighbour;
                                selectedTile.isBlocked = false;
                                visitedTiles.Add(selectedTile);
                                if (selectedTile.leftNeighbour.isBlocked == false)
                                {
                                    while (!selectedTile.leftNeighbour.isBlocked)
                                    {
                                        selectedTile = selectedTile.leftNeighbour;
                                        visitedTiles.Add(selectedTile);
                                        selectedTile.isBlocked = false;
                                    }
                                }






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
                                    if (selectedTile.rightNeighbour.downNeighbour.isBlocked || selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                    {
                                        if (selectedTile.rightNeighbour.downNeighbour.isBlocked && !selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                        {
                                            selectedTile.rightNeighbour.downNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            selectedTile.rightNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        if (selectedTile.rightNeighbour.downNeighbour.isBlocked && selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                        {
                                            selectedTile.rightNeighbour.downNeighbour.isKeyTile = true;
                                            selectedTile.rightNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        selectedTile.rightNeighbour.isKeyTile = false;
                                    }

                                    _pathCreateType = Enums.MoveablePathCreateType.Right;

                                }
                                else
                                {
                                    if (selectedTile.upNeighbour.isBlocked && !selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;


                                    }
                                    else if (!selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Down;

                                    }
                                    /*if (!selectedTile.upNeighbour.isBlocked && !selectedTile.downNeighbour.isBlocked)
                                    {
                                        Debug.Log("-------------SEND FIRST TILE---------------");
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);
                                        while (_pathCreateType == oldPathCreateType)
                                        {
                                            Debug.Log("girdim while");
                                            _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                        }

                                    }*/
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
                            else if (!selectedTile.leftNeighbour.isKeyTile)
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

                    if (!selectedTile.rightNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Left)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(selectedTile, _pathCreateType));
                        if (!selectedTile.rightNeighbour.isKeyTile && selectedTile.rightNeighbour.posOnX != gridWidth - 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

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
                                Debug.Log("Right" + " " + "gittim" + (j + 1));
                                selectedTile = selectedTile.rightNeighbour;
                                selectedTile.isBlocked = false;
                                visitedTiles.Add(selectedTile);
                                if (selectedTile.rightNeighbour.isBlocked == false)
                                {
                                    while (!selectedTile.rightNeighbour.isBlocked)
                                    {
                                        selectedTile = selectedTile.rightNeighbour;
                                        visitedTiles.Add(selectedTile);
                                        selectedTile.isBlocked = false;
                                    }
                                }






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

                                    if (selectedTile.leftNeighbour.downNeighbour.isBlocked || selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                    {
                                        if (selectedTile.leftNeighbour.downNeighbour.isBlocked && !selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                        {
                                            selectedTile.leftNeighbour.downNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            selectedTile.leftNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        if (selectedTile.leftNeighbour.downNeighbour.isBlocked && selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                        {
                                            selectedTile.leftNeighbour.downNeighbour.isKeyTile = true;
                                            selectedTile.leftNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        selectedTile.leftNeighbour.isKeyTile = false;
                                    }
                                    _pathCreateType = Enums.MoveablePathCreateType.Left;

                                }
                                else
                                {
                                    if (selectedTile.upNeighbour.isBlocked && !selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;

                                    }
                                    else if (!selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Down;

                                    }
                                    /*if (!selectedTile.upNeighbour.isBlocked && !selectedTile.downNeighbour.isBlocked)
                                    {
                                        Debug.Log("-------------SEND FIRST TILE---------------");
                                        selectedTile = firstSelectedTile;
                                        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);
                                        while (_pathCreateType == oldPathCreateType)
                                        {
                                            Debug.Log("girdim while");
                                            _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

                                        }

                                    }*/
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
                            if (selectedTile.rightNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

                            }
                            else if (!selectedTile.rightNeighbour.isKeyTile)
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
