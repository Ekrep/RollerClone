using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    public List<Tile> _calculatedPath;
    private void OnEnable()
    {
        GameManager.Slide += GameManager_Slide;
    }

    private void GameManager_Slide(Enums.BallMovementBehaviour movementBehaviour, Tile currentTile)
    {

        _calculatedPath = CalculatePath(movementBehaviour, currentTile);
        GameManager.Instance.OnSendPathToBall(_calculatedPath);

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

                while (currentTile.upNeighbour != null && !currentTile.upNeighbour.isBlocked)
                {
                    path.Add(currentTile.upNeighbour);
                    currentTile = currentTile.upNeighbour;
                    
                }



                break;
            case Enums.BallMovementBehaviour.SwipedDown:

                while (currentTile.downNeighbour != null && !currentTile.downNeighbour.isBlocked)
                {
                    path.Add(currentTile.downNeighbour);
                    currentTile = currentTile.downNeighbour;
                   
                }



                break;
            case Enums.BallMovementBehaviour.SwipedLeft:

                while (currentTile.leftNeighbour != null && !currentTile.leftNeighbour.isBlocked)
                {
                    path.Add(currentTile.leftNeighbour);
                    currentTile = currentTile.leftNeighbour;
                  
                }



                break;
            case Enums.BallMovementBehaviour.SwipedRight:
                while (currentTile.rightNeighbour != null && !currentTile.rightNeighbour.isBlocked)
                {
                    path.Add(currentTile.rightNeighbour);
                    currentTile = currentTile.rightNeighbour;
                   

                }



                break;
            default:
                break;


        }
        return path;


    }
}
