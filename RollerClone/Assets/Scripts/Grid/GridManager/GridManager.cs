using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;


    [SerializeField] private GameObject _tilePrefab;
     public int gridHeight;
     public int gridWidth;
    [SerializeField] private Transform _tileParent;

     public List<Tile> allTiles;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void GenerateGrid()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject go;
                go = Instantiate(_tilePrefab, new Vector3(i * 10, 0, j * 10), Quaternion.identity);
                go.GetComponent<Tile>().posOnX = i*10;
                go.GetComponent<Tile>().posOnZ = j*10;
                go.transform.SetParent(_tileParent);
                go.name = "Tile" + "(" + i*10 + "," + j*10 + ")";
                allTiles.Add(go.GetComponent<Tile>());
            }
        }
        GameManager.Instance.OnTiled();
    }
}
