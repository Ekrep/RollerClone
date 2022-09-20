using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //Gameobject yerine level scripti olustur!
    [SerializeField] private List<GameObject> levels;


    private int _currentLevelIndex;
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
        _currentLevelIndex++;
        LevelControl();
    }

    private void OnDisable()
    {
        GameManager.GameWin -= GameManager_GameWin;
    }







    public static event Action GameWin;

    public static event Action<Enums.BallMovementBehaviour,Tile> Slide;

    public static event Action Tiled;

    public static event Action<Tile> SendPathToBall;



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
        if (_currentLevelIndex != 0)
        {
            levels[_currentLevelIndex - 1].SetActive(false);
            levels[_currentLevelIndex].SetActive(true);

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

    public void OnSendPathToBall(Tile finalTileOfList)
    {
        if (SendPathToBall!=null)
        {
            SendPathToBall(finalTileOfList);
        }
    }
}
