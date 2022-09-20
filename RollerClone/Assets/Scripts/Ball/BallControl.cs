using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Enums.BallState _state;
    private Enums.BallMovementBehaviour _movementBehaviour;
    public float movementSpeed;
    [HideInInspector] public bool canMove;


    private Vector2 _mouseFirstPos;
    private Vector2 _mouseDeltaPos;
    private Vector2 _mouseCurrentPos;


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
            _mouseCurrentPos = Input.mousePosition;
            _mouseDeltaPos = CalculateDeltaPosition(_mouseFirstPos, _mouseCurrentPos);
           
            float deltaX = _mouseDeltaPos.x;
            float deltaY = _mouseDeltaPos.y;
            
            if (Mathf.Abs(deltaX)>Mathf.Abs(deltaY)&&_mouseDeltaPos!=Vector2.zero)
            {
                if (deltaX>0)
                {
                    _movementBehaviour = Enums.BallMovementBehaviour.SwipedRight;
                }
                else
                {
                    _movementBehaviour = Enums.BallMovementBehaviour.SwipedLeft;
                }
            }
            else if(_mouseDeltaPos != Vector2.zero)
            {
                if (deltaY>0)
                {
                    _movementBehaviour = Enums.BallMovementBehaviour.SwipedUp;
                }
                else
                {
                    _movementBehaviour = Enums.BallMovementBehaviour.SwipedDown;
                }
            }
            GameManager.Instance.OnSlide(_movementBehaviour);
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseDeltaPos = Vector2.zero;
            _movementBehaviour = Enums.BallMovementBehaviour.Idle;
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
