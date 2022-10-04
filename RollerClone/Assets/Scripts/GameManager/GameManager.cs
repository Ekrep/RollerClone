using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LevelData levelData;
    //Gameobject yerine level scripti olustur!
    public List<int> levelSeeds;

    public List<int> levelWidth;
    public List<int> levelHeight;


    [HideInInspector] public int currentLevelIndex=0;
    private void Awake()
    {
        Instance = this;
        LoadSeed();
        currentLevelIndex = 0;
        
        
    }
    private void OnEnable()
    {
        currentLevelIndex=PlayerPrefs.GetInt("CurrentLevel");
        GameWin += GameManager_GameWin;
        NextLevel += GameManager_NextLevel;
    }

    private void GameManager_NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
    }

    private void GameManager_GameWin()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levelData.seed.Count)
        {
            currentLevelIndex = 0;
        }
        Debug.Log("win");
        
    }

    private void OnDisable()
    {
        
        GameWin -= GameManager_GameWin;
        NextLevel -= GameManager_NextLevel;
    }
   
    private void LoadSeed()
    {
        int savedSeedCount = PlayerPrefs.GetInt("Count");
        for (int i = 0; i < savedSeedCount; i++)
        {
            int seed = PlayerPrefs.GetInt("Seed"+i, i);
            int height= PlayerPrefs.GetInt("Height" + i, i);
            int width = PlayerPrefs.GetInt("Width" + i, i);
            levelSeeds.Add(seed);
            levelHeight.Add(height);
            levelWidth.Add(width);
        }
    }



    #region Game Events

    public static event Action GameWin;

    public static event Action NextLevel;

    public static event Action<Enums.BallMovementBehaviour, Tile> Slide;

    public static event Action Tiled;

    public static event Action<List<Tile>> SendPathToBall;

    public static event Action<Tile> SendStartPosToBall;

    public static event Action<int> SendRequiredTilesToDye;

    #endregion

    #region UI Events

    public static event Action WinPanelOpen;
    public static event Action WinPanelClose;

    public static event Action StartPanelOpen;
    public static event Action StartPanelClose;

    #endregion

    #region LevelGenEvent
    public static event Action<int> GetSeed;
    #endregion

    #region LevelGenFunction
    public void OnGetSeed(int seed)
    {
        if (GetSeed!=null)
        {
            GetSeed(seed);
        }
    }
    #endregion







    #region Game Event Functions
    public void OnGameWin()
    {
        if (GameWin != null)
        {
            GameWin();
        }
    }

    public void OnNextLevel()
    {
        if (NextLevel != null)
        {
            NextLevel();
        }
    }

    public void OnSlide(Enums.BallMovementBehaviour movementBehaviour, Tile currentTile)
    {
        if (Slide != null)
        {
            Slide(movementBehaviour, currentTile);
        }
    }

    public void OnTiled()
    {
        if (Tiled != null)
        {
            Tiled();
        }
    }

    public void OnSendPathToBall(List<Tile> tilePath)
    {
        if (SendPathToBall != null)
        {
            SendPathToBall(tilePath);
        }
    }
    public void OnSendStartPosToBall(Tile startTile)
    {
        if (SendStartPosToBall != null)
        {
            SendStartPosToBall(startTile);
        }
    }

    public void OnSendRequiredTilesToDye(int unblockedTilesCount)
    {
        if (SendRequiredTilesToDye != null)
        {
            SendRequiredTilesToDye(unblockedTilesCount);
        }
    }
    #endregion

    #region UI Event Functions
    public void OnWinPanelOpen()
    {
        if (WinPanelOpen!=null)
        {
            WinPanelOpen();
        }
    }
    public void OnWinPanelClose()
    {
        if (WinPanelClose != null)
        {
            WinPanelClose();
        }
    }

    public void OnStartPanelOpen()
    {
        if (StartPanelOpen!=null)
        {
            StartPanelOpen();
        }
    }

    public void OnStartPanelClose()
    {
        if (StartPanelClose!=null)
        {
            StartPanelClose();
        }
    }



    #endregion
}
