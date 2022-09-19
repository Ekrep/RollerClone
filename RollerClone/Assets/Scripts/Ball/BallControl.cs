using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Enums.BallState _state;
    public float movementSpeed;
    [HideInInspector] public bool canMove;


    private Vector2 _mouseFirstPos;
    private Vector2 _mouseDeltaPos;
    private Vector2 _mouseLastPos;


    private void Update()
    {
        MoveBall();
        

    }


    private void MoveBall()
    {


        if (Input.GetMouseButtonDown(0))
        {
            _mouseFirstPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _mouseLastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            _mouseDeltaPos = CalculateDeltaPosition(_mouseFirstPos.normalized, _mouseLastPos.normalized);
            Debug.Log(_mouseDeltaPos + "DeltaPos");
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseDeltaPos = Vector2.zero;
        }

    }


    private Vector2 CalculateDeltaPosition(Vector2 firstPos, Vector2 secondPos)
    {
        Vector2 distanceVector;
        distanceVector = secondPos - firstPos;

        return distanceVector;
    }

    private Enums.BallState CalculateMovementBehaviour()
    {


        return 0;
    }

}
