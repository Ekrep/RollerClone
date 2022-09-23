using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
   
    private Enums.BallState _state;
    private Enums.BallMovementBehaviour _movementBehaviour;
    public float movementSpeed;
    [HideInInspector] public bool canMove;

    [SerializeField] private Tile _currentTile;


    private List<Tile> _ballTargetPath;

    [SerializeField]private int _pathWayCurrentIndex = 0;


    private Vector2 _mouseFirstPos;
    private Vector2 _mouseDeltaPos;
    private Vector2 _mouseCurrentPos;

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
        GameManager.SendPathToBall += GameManager_SendPathToBall;
        GameManager.SendStartPosToBall += GameManager_SendStartPosToBall;
    }

    private void GameManager_SendStartPosToBall(Tile startTile)
    {
        gameObject.transform.position = new Vector3(startTile.transform.position.x, 0.5f, startTile.transform.position.z);
        _currentTile = startTile;
    }

    private void GameManager_SendPathToBall(List<Tile> tilePath)
    {
        StartCoroutine(MoveBallToTarget(tilePath));
        _currentTile = tilePath[tilePath.Count - 1];
        _ballTargetPath = tilePath;
    }

    private void GameManager_Tiled()
    {
        
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
        GameManager.SendPathToBall -= GameManager_SendPathToBall;
        GameManager.SendStartPosToBall -= GameManager_SendStartPosToBall;
    }


    private void Update()
    {
        MoveBall();
        Debug.Log(_state);

    }

   


    private void MoveBall()
    {
        if (_state != Enums.BallState.Moving)
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

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) && _mouseDeltaPos != Vector2.zero)
                {
                    if (deltaX > 0)
                    {
                        _movementBehaviour = Enums.BallMovementBehaviour.SwipedRight;
                    }
                    else
                    {
                        _movementBehaviour = Enums.BallMovementBehaviour.SwipedLeft;
                    }
                }
                else if (_mouseDeltaPos != Vector2.zero)
                {
                    if (deltaY > 0)
                    {
                        _movementBehaviour = Enums.BallMovementBehaviour.SwipedUp;
                    }
                    else
                    {
                        _movementBehaviour = Enums.BallMovementBehaviour.SwipedDown;
                    }
                }


            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_mouseDeltaPos != Vector2.zero)
                {
                    GameManager.Instance.OnSlide(_movementBehaviour, _currentTile);
                    _mouseDeltaPos = Vector2.zero;
                    _state = Enums.BallState.Moving;
                    _movementBehaviour = Enums.BallMovementBehaviour.Idle;
                }

            }
        }



    }


    private Vector2 CalculateDeltaPosition(Vector2 firstPos, Vector2 secondPos)
    {
        Vector2 distanceVector;
        distanceVector = secondPos - firstPos;

        return distanceVector;
    }


 
    IEnumerator MoveBallToTarget(List<Tile> path)
    {
        
        yield return new WaitForFixedUpdate();
        if (gameObject.transform.position != path[path.Count-1].transform.position)
        {

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, path[_pathWayCurrentIndex].transform.position, movementSpeed * Time.deltaTime);
            StartCoroutine(MoveBallToTarget(path));
            if (gameObject.transform.position==path[_pathWayCurrentIndex].transform.position&&
                _pathWayCurrentIndex<path.Count)
            {
                
                path[_pathWayCurrentIndex].tileColor.material.color = Color.blue;
                _pathWayCurrentIndex++;
            }
        }
        else
        {
            
            
                _state = Enums.BallState.Idle;
                StopCoroutine(MoveBallToTarget(path));
                _pathWayCurrentIndex = 0;
            

        }
    }



}
