using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //Gameobject yerine level scripti olustur!
    [SerializeField] private List<GameObject> levels;


    [HideInInspector]public int currentLevelIndex;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.GameWin += GameManager_GameWin;
    }

    private void GameManager_GameWin()
    {
        currentLevelIndex++;
        LevelControl();
    }

    private void OnDisable()
    {
        GameManager.GameWin -= GameManager_GameWin;
    }







    public static event Action GameWin;

    public static event Action<Enums.BallMovementBehaviour,Tile> Slide;

    public static event Action Tiled;

    public static event Action<List<Tile>> SendPathToBall;

    public static event Action<Tile> SendStartPosToBall;



    #region LevelControl Function
    private void LevelControl()
    {
        /*for (int i = 0; i < levels.Count; i++)
        {
            if (i!=_currentLevelIndex)
            {
                levels[i].SetActive(false);
            }
        }*/
        if (currentLevelIndex != 0)
        {
            levels[currentLevelIndex - 1].SetActive(false);
            levels[currentLevelIndex].SetActive(true);

        }

    }



    #endregion


    public void OnGameWin()
    {
        if (GameWin != null)
        {
            GameWin();
        }
    }

    public void OnSlide(Enums.BallMovementBehaviour movementBehaviour, Tile currentTile)
    {
        if (Slide != null)
        {
            Slide(movementBehaviour,currentTile);
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
        if (SendPathToBall!=null)
        {
            SendPathToBall(tilePath);
        }
    }
    public void OnSendStartPosToBall(Tile startTile)
    {
        if (SendStartPosToBall!=null)
        {
            SendStartPosToBall(startTile);
        }
    }
}
