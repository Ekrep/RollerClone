using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int _seed;

    public List<int> levelSeeds=new List<int>();
    public List<int> levelWidth = new List<int>();
    public List<int> levelHeight = new List<int>();
    private int savedSeedCount;
    private void OnEnable()
    {
        GameManager.GetSeed += GameManager_GetSeed;
    }

    private void GameManager_GetSeed(int seed)
    {
        _seed = seed;
        
    }

    private void OnDisable()
    {
        GameManager.GetSeed -= GameManager_GetSeed;
    }
    private void Awake()
    {
        Debug.Log(Random.seed + "seedlevelgen");
    }


    public void SaveSeed()
    {
        levelSeeds.Add(_seed);
        levelHeight.Add(GridManager.Instance.gridHeight);
        levelWidth.Add(GridManager.Instance.gridWidth);
        PlayerPrefs.SetInt("Count", levelSeeds.Count);
        for (int i = 0; i < levelSeeds.Count; i++)
        {
            
            PlayerPrefs.SetInt("Seed"+i, levelSeeds[i]);
            PlayerPrefs.SetInt("Height" + i, levelHeight[i]);
            PlayerPrefs.SetInt("Width" + i, levelWidth[i]);

        }
       
    }  
    

    /*public void LoadSeed()
    {
        levelSeeds.Clear();
        savedSeedCount = PlayerPrefs.GetInt("Count");
        for (int i = 0; i < savedSeedCount; i++)
        {
            Debug.Log("girdim load seed");
            int seed = PlayerPrefs.GetInt("Seed"+i, i);
            levelSeeds.Add(seed);
        }
    }*/
}

