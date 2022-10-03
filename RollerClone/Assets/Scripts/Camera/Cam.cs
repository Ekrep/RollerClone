using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
        GameManager.NextLevel += GameManager_NextLevel;
    }

    private void GameManager_NextLevel()
    {
        this.gameObject.transform.position = new Vector3((float)(GridManager.Instance.gridWidth / 2) * 10 - 5, (float)Screen.width / (GameManager.Instance.levelWidth[GameManager.Instance.currentLevelIndex] / 3f), (float)(GridManager.Instance.gridHeight / 4) * 10);
    }

    private void GameManager_Tiled()
    {
       
       // this.gameObject.transform.position = new Vector3((float)(GridManager.Instance.gridWidth / 2)*10-5, (float) Screen.width/(GridManager.Instance.gridWidth/3), (float)(GridManager.Instance.gridHeight / 4)*10);
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
        GameManager.NextLevel -= GameManager_NextLevel;
    }

    private void Start()
    {
        this.gameObject.transform.position = new Vector3((float)(GridManager.Instance.gridWidth / 2) * 10 - 5, (float)Screen.width / (GameManager.Instance.levelWidth[GameManager.Instance.currentLevelIndex] / 3f), (float)(GridManager.Instance.gridHeight / 4) * 10);
    }

}
