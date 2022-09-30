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


    private Tile _selectedTile;

    public List<Tile> keyTiles;







    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        GenerateObstacles();
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
                unblockedTiles.Add(allTiles[i]);
                //allTiles[i].tileColor.material.color = Color.white;
            }
        }
        GameManager.Instance.OnSendRequiredTilesToDye(unblockedTiles.Count);
    }


    private void CreateRealPathThisTime()
    {

        _selectedTile = tileDictionary[new Vector2Int(Random.Range(1, gridWidth - 2), Random.Range(1, gridHeight - 2))];
        Debug.Log("/////////FIRST SELECTED TILE//////////" + _selectedTile);
        GameManager.Instance.OnSendStartPosToBall(_selectedTile);
        Tile firstSelectedTile = _selectedTile;



        _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 4);

        Enums.MoveablePathCreateType oldPathCreateType = Enums.MoveablePathCreateType.Base;

        List<Tile> unblockedTiles = new List<Tile>();

        /*if (_pathCreateType==Enums.MoveablePathCreateType.Up)
        {
            selectedTile.leftNeighbour.isKeyTile = true;
        }
        if (_pathCreateType == Enums.MoveablePathCreateType.Down)
        {
            selectedTile.leftNeighbour.isKeyTile = true;
        }
        if (_pathCreateType == Enums.MoveablePathCreateType.Left)
        {
            selectedTile.upNeighbour.isKeyTile = true;
        }
        if (_pathCreateType == Enums.MoveablePathCreateType.Right)
        {
            selectedTile.upNeighbour.isKeyTile = true;
        }*/

        //gittiði yönün tersine gitmeme ekle
        int iterationCount = 100;
        int pathWayIterationCount;
        for (int i = 0; i < iterationCount; i++)
        {
            switch (_pathCreateType)
            {
                case Enums.MoveablePathCreateType.Up:
                    Debug.Log("----------TRY'N UP--------------");

                    if (!_selectedTile.upNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Down)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = _selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(_selectedTile, _pathCreateType));
                        if (!_selectedTile.upNeighbour.isKeyTile && _selectedTile.upNeighbour.posOnZ != gridHeight - 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

                        _selectedTile.isBlocked = false;
                        if (_selectedTile.downNeighbour.isBlocked)
                        {
                            _selectedTile.downNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Up" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!_selectedTile.upNeighbour.isKeyTile && _selectedTile.upNeighbour.posOnZ != gridHeight - 1)
                            {

                                Debug.Log("Up" + " " + "gittim" + (j + 1));
                                _selectedTile = _selectedTile.upNeighbour;
                                _selectedTile.isBlocked = false;
                                visitedTiles.Add(_selectedTile);
                                if (_selectedTile.upNeighbour.isBlocked == false)
                                {
                                    while (!_selectedTile.upNeighbour.isBlocked)
                                    {
                                        _selectedTile = _selectedTile.upNeighbour;
                                        visitedTiles.Add(_selectedTile);
                                        _selectedTile.isBlocked = false;
                                    }
                                }







                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("UPBREAK");
                                //holder.downNeighbour.isKeyTile = false;
                                if (_selectedTile.downNeighbour.isBlocked)
                                {
                                    _selectedTile.upNeighbour.isKeyTile = true;
                                }
                                if ( visitedTiles.Count < 2 && _selectedTile.downNeighbour.isBlocked)
                                {
                                    if (_selectedTile.downNeighbour.leftNeighbour.isBlocked || _selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                    {
                                        if (_selectedTile.downNeighbour.leftNeighbour.isBlocked && !_selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                        {
                                            _selectedTile.downNeighbour.leftNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            _selectedTile.downNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        if (_selectedTile.downNeighbour.leftNeighbour.isBlocked && _selectedTile.downNeighbour.rightNeighbour.isBlocked)
                                        {
                                            _selectedTile.downNeighbour.leftNeighbour.isKeyTile = true;
                                            _selectedTile.downNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        _selectedTile.downNeighbour.isKeyTile = false;

                                    }

                                    _pathCreateType = Enums.MoveablePathCreateType.Down;

                                }
                                else
                                {
                                    if (_selectedTile.leftNeighbour.isBlocked && !_selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;

                                    }
                                    else if (!_selectedTile.leftNeighbour.isKeyTile)
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
                        if (_selectedTile.upNeighbour.isBlocked)
                        {
                            _selectedTile.upNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (_selectedTile.upNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);

                            }
                            else if (!_selectedTile.upNeighbour.isKeyTile)
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

                    if (!_selectedTile.downNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Up)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = _selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(_selectedTile, _pathCreateType));
                        if (!_selectedTile.downNeighbour.isKeyTile && _selectedTile.downNeighbour.posOnZ >= 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

                        _selectedTile.isBlocked = false;
                        if (_selectedTile.upNeighbour.isBlocked)
                        {
                            _selectedTile.upNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Down" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!_selectedTile.downNeighbour.isKeyTile && _selectedTile.downNeighbour.posOnZ >= 1)
                            {
                                Debug.Log("Down" + " " + "gittim" + (j + 1));
                                _selectedTile = _selectedTile.downNeighbour;
                                _selectedTile.isBlocked = false;
                                visitedTiles.Add(_selectedTile);
                                if (_selectedTile.downNeighbour.isBlocked == false)
                                {
                                    while (!_selectedTile.downNeighbour.isBlocked)
                                    {
                                        _selectedTile = _selectedTile.downNeighbour;
                                        visitedTiles.Add(_selectedTile);
                                        _selectedTile.isBlocked = false;
                                    }
                                }






                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("DOWNBREAK");
                                //holder.upNeighbour.isKeyTile = false;
                                if (_selectedTile.downNeighbour.isBlocked)
                                {
                                    _selectedTile.downNeighbour.isKeyTile = true;
                                }
                                if ( visitedTiles.Count < 2 && _selectedTile.upNeighbour.isBlocked)
                                {
                                    if (_selectedTile.upNeighbour.leftNeighbour.isBlocked || _selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                    {
                                        if (_selectedTile.upNeighbour.leftNeighbour.isBlocked && !_selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                        {
                                            _selectedTile.upNeighbour.leftNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            _selectedTile.upNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        if (_selectedTile.upNeighbour.leftNeighbour.isBlocked && _selectedTile.upNeighbour.rightNeighbour.isBlocked)
                                        {
                                            _selectedTile.upNeighbour.leftNeighbour.isKeyTile = true;
                                            _selectedTile.upNeighbour.rightNeighbour.isKeyTile = true;
                                        }
                                        _selectedTile.upNeighbour.isKeyTile = false;

                                    }


                                    _pathCreateType = Enums.MoveablePathCreateType.Up;

                                }
                                else
                                {
                                    if (_selectedTile.leftNeighbour.isBlocked && !_selectedTile.leftNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Left;

                                    }
                                    else if (!_selectedTile.leftNeighbour.isKeyTile)
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
                        if (_selectedTile.downNeighbour.isBlocked)
                        {
                            _selectedTile.downNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (_selectedTile.downNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(2, 4);

                            }
                            else if (!_selectedTile.downNeighbour.isKeyTile)
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

                    if (!_selectedTile.leftNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Right)
                    {
                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        holder = _selectedTile;
                        bool isBreak = false;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(_selectedTile, _pathCreateType));
                        _selectedTile.isBlocked = false;
                        if (!_selectedTile.leftNeighbour.isKeyTile && _selectedTile.leftNeighbour.posOnX >= 1)
                        {
                            oldPathCreateType = _pathCreateType;

                        }

                        if (_selectedTile.rightNeighbour.isBlocked)
                        {
                            _selectedTile.rightNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Left" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!_selectedTile.leftNeighbour.isKeyTile && _selectedTile.leftNeighbour.posOnX >= 1)
                            {
                                Debug.Log("Left" + " " + "gittim" + (j + 1));
                                _selectedTile = _selectedTile.leftNeighbour;
                                _selectedTile.isBlocked = false;
                                visitedTiles.Add(_selectedTile);
                                if (_selectedTile.leftNeighbour.isBlocked == false)
                                {
                                    while (!_selectedTile.leftNeighbour.isBlocked)
                                    {
                                        _selectedTile = _selectedTile.leftNeighbour;
                                        visitedTiles.Add(_selectedTile);
                                        _selectedTile.isBlocked = false;
                                    }
                                }






                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("LEFTBREAK");
                                //holder.rightNeighbour.isKeyTile = false;
                                if (_selectedTile.leftNeighbour.isBlocked)
                                {
                                    _selectedTile.leftNeighbour.isKeyTile = true;
                                }
                                if ( visitedTiles.Count < 2 && _selectedTile.rightNeighbour.isBlocked)
                                {
                                    if (_selectedTile.rightNeighbour.downNeighbour.isBlocked || _selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                    {
                                        if (_selectedTile.rightNeighbour.downNeighbour.isBlocked && !_selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                        {
                                            _selectedTile.rightNeighbour.downNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            _selectedTile.rightNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        if (_selectedTile.rightNeighbour.downNeighbour.isBlocked && _selectedTile.rightNeighbour.upNeighbour.isBlocked)
                                        {
                                            _selectedTile.rightNeighbour.downNeighbour.isKeyTile = true;
                                            _selectedTile.rightNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        _selectedTile.rightNeighbour.isKeyTile = false;
                                    }

                                    _pathCreateType = Enums.MoveablePathCreateType.Right;

                                }
                                else
                                {
                                    if (_selectedTile.upNeighbour.isBlocked && !_selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;


                                    }
                                    else if (!_selectedTile.upNeighbour.isKeyTile)
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
                        if (_selectedTile.leftNeighbour.isBlocked)
                        {
                            _selectedTile.leftNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (_selectedTile.leftNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

                            }
                            else if (!_selectedTile.leftNeighbour.isKeyTile)
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

                    if (!_selectedTile.rightNeighbour.isKeyTile && oldPathCreateType != Enums.MoveablePathCreateType.Left)
                    {

                        List<Tile> visitedTiles = new List<Tile>();
                        Tile holder;
                        bool isBreak = false;
                        holder = _selectedTile;
                        pathWayIterationCount = Random.Range(1, FindRandomDir(_selectedTile, _pathCreateType));
                        if (!_selectedTile.rightNeighbour.isKeyTile && _selectedTile.rightNeighbour.posOnX != gridWidth - 1)
                        {
                            oldPathCreateType = _pathCreateType;
                        }

                        _selectedTile.isBlocked = false;
                        if (_selectedTile.leftNeighbour.isBlocked)
                        {
                            _selectedTile.leftNeighbour.isKeyTile = true;
                        }

                        Debug.Log("Right" + " " + pathWayIterationCount);
                        for (int j = 0; j < pathWayIterationCount; j++)
                        {
                            if (!_selectedTile.rightNeighbour.isKeyTile && _selectedTile.rightNeighbour.posOnX != gridWidth - 1)
                            {
                                Debug.Log("Right" + " " + "gittim" + (j + 1));
                                _selectedTile = _selectedTile.rightNeighbour;
                                _selectedTile.isBlocked = false;
                                visitedTiles.Add(_selectedTile);
                                if (_selectedTile.rightNeighbour.isBlocked == false)
                                {
                                    while (!_selectedTile.rightNeighbour.isBlocked)
                                    {
                                        _selectedTile = _selectedTile.rightNeighbour;
                                        visitedTiles.Add(_selectedTile);
                                        _selectedTile.isBlocked = false;
                                    }
                                }






                            }
                            else
                            {
                                isBreak = true;
                                Debug.Log("RIGHTBREAK");
                                //holder.leftNeighbour.isKeyTile = false;
                                if (_selectedTile.rightNeighbour.isBlocked)
                                {
                                    _selectedTile.rightNeighbour.isKeyTile = true;
                                }
                                if ( visitedTiles.Count < 2 && _selectedTile.leftNeighbour.isBlocked)
                                {

                                    if (_selectedTile.leftNeighbour.downNeighbour.isBlocked || _selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                    {
                                        if (_selectedTile.leftNeighbour.downNeighbour.isBlocked && !_selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                        {
                                            _selectedTile.leftNeighbour.downNeighbour.isKeyTile = true;
                                        }
                                        else
                                        {
                                            _selectedTile.leftNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        if (_selectedTile.leftNeighbour.downNeighbour.isBlocked && _selectedTile.leftNeighbour.upNeighbour.isBlocked)
                                        {
                                            _selectedTile.leftNeighbour.downNeighbour.isKeyTile = true;
                                            _selectedTile.leftNeighbour.upNeighbour.isKeyTile = true;
                                        }
                                        _selectedTile.leftNeighbour.isKeyTile = false;
                                    }
                                    _pathCreateType = Enums.MoveablePathCreateType.Left;

                                }
                                else
                                {
                                    if (_selectedTile.upNeighbour.isBlocked && !_selectedTile.upNeighbour.isKeyTile)
                                    {
                                        _pathCreateType = Enums.MoveablePathCreateType.Up;

                                    }
                                    else if (!_selectedTile.upNeighbour.isKeyTile)
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
                        if (_selectedTile.rightNeighbour.isBlocked)
                        {
                            _selectedTile.rightNeighbour.isKeyTile = true;
                        }
                        if (!isBreak)
                        {
                            if (_selectedTile.rightNeighbour.isBlocked)
                            {
                                _pathCreateType = (Enums.MoveablePathCreateType)Random.Range(0, 2);

                            }
                            else if (!_selectedTile.rightNeighbour.isKeyTile)
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
