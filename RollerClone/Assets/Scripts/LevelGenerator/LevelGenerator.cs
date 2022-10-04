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
    public LevelData levelData;
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
        int seed=0;
        int height=0;
        int width=0;
        levelSeeds.Clear();
        levelHeight.Clear();
        levelWidth.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt("Count",0); i++)
        {
            Debug.Log("firstAdd");
            levelSeeds.Add(PlayerPrefs.GetInt("Seed" + i, 0));
            levelHeight.Add(PlayerPrefs.GetInt("Height" + i, 0));
            levelWidth.Add(PlayerPrefs.GetInt("Width" + i, 0));
             seed = PlayerPrefs.GetInt("Seed" + i, i);
             height = PlayerPrefs.GetInt("Height" + i, i);
             width = PlayerPrefs.GetInt("Width" + i, i);
           
        }
        

        levelSeeds.Add(_seed);
        levelHeight.Add(GridManager.Instance.gridHeight);
        levelWidth.Add(GridManager.Instance.gridWidth);
        levelData.gridHeight.Add(height);
        levelData.gridWith.Add(width);
        levelData.seed.Add(seed);

        for (int i = 0; i < levelSeeds.Count; i++)
        {
            Debug.Log("secondAdd");
            PlayerPrefs.SetInt("Seed" + i, levelSeeds[i]);
            PlayerPrefs.SetInt("Height" + i, levelHeight[i]);
            PlayerPrefs.SetInt("Width" + i, levelWidth[i]);
            



        }
        PlayerPrefs.SetInt("Count", levelSeeds.Count);

       
        

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

