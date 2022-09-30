using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{


    public AnimationCurve curve;

    private Enums.BallState _state;
    private Enums.BallMovementBehaviour _movementBehaviour;
    public float movementSpeed;
    private bool _canMove;

    [SerializeField] private Tile _currentTile;


    [SerializeField]private int _pathWayCurrentIndex = 0;

    private int _requiredTilesToDye;

    private int _coloredTileCount;

    private float _ballJumpSpeed;

    private float _timeScaleForJump=0f;


    private Vector2 _mouseFirstPos;
    private Vector2 _mouseDeltaPos;
    private Vector2 _mouseCurrentPos;

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
        GameManager.SendPathToBall += GameManager_SendPathToBall;
        GameManager.SendStartPosToBall += GameManager_SendStartPosToBall;
        GameManager.SendRequiredTilesToDye += GameManager_SendRequiredTilesToDye;
        GameManager.GameWin += GameManager_GameWin;
    }

    private void GameManager_GameWin()
    {
        _canMove = false;
        CallBallJump();
        
    }

    private void GameManager_SendRequiredTilesToDye(int requiredTilesToDye)
    {
        _requiredTilesToDye = requiredTilesToDye;
    }

    private void GameManager_SendStartPosToBall(Tile startTile)
    {
        gameObject.transform.position = new Vector3(startTile.transform.position.x, 1f, startTile.transform.position.z);
        _currentTile = startTile;
    }

    private void GameManager_SendPathToBall(List<Tile> tilePath)
    {
        StartCoroutine(MoveBallToTarget(tilePath));
        _currentTile = tilePath[tilePath.Count - 1];
        
    }

    private void GameManager_Tiled()
    {
        
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
        GameManager.SendPathToBall -= GameManager_SendPathToBall;
        GameManager.SendStartPosToBall -= GameManager_SendStartPosToBall;
        GameManager.SendRequiredTilesToDye -= GameManager_SendRequiredTilesToDye;
        GameManager.GameWin -= GameManager_GameWin;
    }
    private void Start()
    {
        _canMove = true;
    }

    private void Update()
    {
        MoveBall();
        //Debug.Log(_state);

    }

   


    private void MoveBall()
    {
        if (_canMove)
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
        if (_canMove)
        {
            if (gameObject.transform.position != new Vector3(path[path.Count - 1].transform.position.x, 1, path[path.Count - 1].transform.position.z))
            {
                Debug.Log("movecoroutine");
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(path[_pathWayCurrentIndex].transform.position.x, 1, path[_pathWayCurrentIndex].transform.position.z), movementSpeed * Time.deltaTime);
                StartCoroutine(MoveBallToTarget(path));
                if (gameObject.transform.position == new Vector3(path[_pathWayCurrentIndex].transform.position.x, 1, path[_pathWayCurrentIndex].transform.position.z) &&
                    _pathWayCurrentIndex < path.Count)
                {
                    if (path[_pathWayCurrentIndex].tileColor.material.color != Color.blue)
                    {
                        path[_pathWayCurrentIndex].tileColor.material.color = Color.blue;
                        _coloredTileCount++;

                    }

                    _pathWayCurrentIndex++;
                }
            }
            else
            {


                _state = Enums.BallState.Idle;
                StopCoroutine(MoveBallToTarget(path));
                _pathWayCurrentIndex = 0;
                if (_coloredTileCount == _requiredTilesToDye)
                {
                    GameManager.Instance.OnGameWin();
                }


            }
        }
       
    }

    private void CallBallJump()
    {
        StartCoroutine(BallJump());
    }
    IEnumerator BallJump()
    {
        yield return new WaitForFixedUpdate();
        if (gameObject.transform.position.y<10f)
        {
            _ballJumpSpeed = curve.Evaluate(_timeScaleForJump);
            _timeScaleForJump += Time.deltaTime/10f;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, 10f, gameObject.transform.position.z), _ballJumpSpeed*100f * Time.deltaTime);
            StartCoroutine(BallJump());
        }
        else
        {
            _timeScaleForJump = 0f;
            StopCoroutine(BallJump());
            StartCoroutine(ReplaceBall());
        }

    }

    IEnumerator ReplaceBall()
    {
        yield return new WaitForFixedUpdate();

        if (gameObject.transform.position.y>1f)
        {
           
            _ballJumpSpeed = curve.Evaluate(_timeScaleForJump);
            _timeScaleForJump += Time.deltaTime/10f;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, 1f, gameObject.transform.position.z),_ballJumpSpeed*100f*Time.deltaTime);
            StartCoroutine(ReplaceBall());
        }
        else
        {
            _timeScaleForJump = 0f;
            StopCoroutine(ReplaceBall());
        }

    }

}
