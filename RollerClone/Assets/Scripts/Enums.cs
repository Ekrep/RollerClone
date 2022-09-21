using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public static Enums Instance;

    private void Awake()
    {
        Instance = this;
    }

    public enum BallMovementBehaviour
    {
        Idle,
        SwipedUp,
        SwipedDown,
        SwipedLeft,
        SwipedRight
    }
    public enum BallState
    {
        Idle,
        Moving
        
    }

    public enum TileType
    {
        Moveable,
        Blocked

    }

   public enum MoveablePathCreateType
    {
        Up,
        Down,
        Left,
        Right
    }

}
