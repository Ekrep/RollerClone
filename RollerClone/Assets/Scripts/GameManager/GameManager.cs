using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }




    public static event Action GameWin;

    public static event Action Slide;

    public static event Action Tiled;

    public void OnGameWin()
    {
        if (GameWin != null)
        {
            GameWin();
        }
    }

    public void OnSlide()
    {
        if (Slide != null)
        {
            Slide();
        }
    }

    public void OnTiled()
    {
        if (Tiled!=null)
        {
            Tiled();
        }
    }
}
