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

    public enum BallState
    {
        SwipedUp,
        SwipedDown,
        SwipedLeft,
        SwipedRight
    }

}
