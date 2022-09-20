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


    private Vector2 _mouseFirstPos;
    private Vector2 _mouseDeltaPos;
    private Vector2 _mouseCurrentPos;

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
        GameManager.SendPathToBall += GameManager_SendPathToBall;
    }

    private void GameManager_SendPathToBall(Tile finalPointOfPath)
    {
        StartCoroutine(MoveBallToTarget(finalPointOfPath.transform));
        _currentTile = finalPointOfPath;
    }

    private void GameManager_Tiled()
    {
        gameObject.transform.position = new Vector3(GridManager.Instance.allTiles[0].transform.position.x, 0.5f, GridManager.Instance.allTiles[0].transform.position.z);
        _currentTile = GridManager.Instance.allTiles[0];
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
        GameManager.SendPathToBall -= GameManager_SendPathToBall;
    }


    private void Update()
    {
        MoveBall();
        Debug.Log(_state);

    }

    private void OnCollisionEnter(Collision collision)
    {
       /* if (collision.gameObject.CompareTag("Tile")&&collision.gameObject.GetComponent<Tile>().isBlocked==false)
        {
            _currentTile = collision.gameObject.GetComponent<Tile>();
        }*/
    }


    private void MoveBall()
    {
        if (_state!=Enums.BallState.Moving)
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
                if (_mouseDeltaPos!=Vector2.zero)
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

  

    IEnumerator MoveBallToTarget(Transform target)
    {
        
        yield return new WaitForFixedUpdate();
        if (gameObject.transform.position!=target.position)
        {
           
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, movementSpeed * Time.deltaTime);
            StartCoroutine(MoveBallToTarget(target));
        }
        else
        {
            _state = Enums.BallState.Idle;
            StopCoroutine(MoveBallToTarget(target));
        }
    }

}
