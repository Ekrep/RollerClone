using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Slide += GameManager_Slide;
    }

    private void GameManager_Slide(Enums.BallMovementBehaviour obj)
    {
        Debug.Log("girdim" + obj);
        switch (obj)
        {
            case Enums.BallMovementBehaviour.Idle:
                break;
            case Enums.BallMovementBehaviour.SwipedUp:
                break;
            case Enums.BallMovementBehaviour.SwipedDown:
                break;
            case Enums.BallMovementBehaviour.SwipedLeft:
                break;
            case Enums.BallMovementBehaviour.SwipedRight:
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        GameManager.Slide -= GameManager_Slide;
    }

  
}
