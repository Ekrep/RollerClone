using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    public List<Tile> testList;
    private void OnEnable()
    {
        GameManager.Slide += GameManager_Slide;
    }

    private void GameManager_Slide(Enums.BallMovementBehaviour movementBehaviour, Tile currentTile)
    {

        testList = CalculatePath(movementBehaviour, currentTile);

    }
    private void OnDisable()
    {
        GameManager.Slide -= GameManager_Slide;
    }

    public List<Tile> CalculatePath(Enums.BallMovementBehaviour movementBehaviour, Tile currentTile)
    {
        List<Tile> path = new List<Tile>();

        path.Add(currentTile);

        switch (movementBehaviour)
        {
            case Enums.BallMovementBehaviour.Idle:
                return null;


            case Enums.BallMovementBehaviour.SwipedUp:

                while (currentTile.upNeighbour != null)
                {
                    path.Add(currentTile.upNeighbour);
                    currentTile = currentTile.upNeighbour;
                    Debug.Log("girdim");
                    Debug.Log(currentTile);
                }



                break;
            case Enums.BallMovementBehaviour.SwipedDown:

                while (currentTile.downNeighbour != null)
                {
                    path.Add(currentTile.downNeighbour);
                    currentTile = currentTile.downNeighbour;
                    Debug.Log(currentTile);
                    Debug.Log("girdim");
                }



                break;
            case Enums.BallMovementBehaviour.SwipedLeft:

                while (currentTile.leftNeighbour != null)
                {
                    path.Add(currentTile.leftNeighbour);
                    currentTile = currentTile.leftNeighbour;
                    Debug.Log("girdim");
                    Debug.Log(currentTile);
                }



                break;
            case Enums.BallMovementBehaviour.SwipedRight:
                while (currentTile.rightNeighbour != null)
                {
                    path.Add(currentTile.rightNeighbour);
                    currentTile = currentTile.rightNeighbour;
                    Debug.Log("girdim");
                    Debug.Log(currentTile);

                }



                break;
            default:
                break;


        }
        return path;


    }
}
