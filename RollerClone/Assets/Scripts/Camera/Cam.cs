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
       
        //Camera.main.fieldOfView = GridManager.Instance.gridWidth * 5000 / Screen.width;
        //this.gameObject.transform.position = new Vector3((float)(GameManager.Instance.levelData.gridWith[GameManager.Instance.currentLevelIndex]/ 2) * 10 - 5, (float)Screen.width / (GameManager.Instance.levelData.gridWith[GameManager.Instance.currentLevelIndex] / 3f), (float)(GameManager.Instance.levelData.gridHeight[GameManager.Instance.currentLevelIndex] / 2 / 6) * 10);
    }

    private void GameManager_Tiled()
    {
       
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
        GameManager.NextLevel -= GameManager_NextLevel;
    }

    private void Start()
    {

            Camera.main.aspect = 9f / 16f;
        //Camera.main.fieldOfView = GridManager.Instance.gridWidth * 5000 / Screen.width;
        this.gameObject.transform.position = new Vector3((float)(GameManager.Instance.levelData.gridWith[GameManager.Instance.currentLevelIndex] / 2) * 10 - 5, (float)Screen.width / (GameManager.Instance.levelData.gridWith[GameManager.Instance.currentLevelIndex] / 4f), (float)(GameManager.Instance.levelData.gridHeight[GameManager.Instance.currentLevelIndex] / 2 / 6) * 10);


    }

}
