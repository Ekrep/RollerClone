using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    

    private void OnEnable()
    {
        GameManager.Tiled += GameManager_Tiled;
    }

    private void GameManager_Tiled()
    {
        this.gameObject.transform.position = new Vector3((float)(GridManager.Instance.gridWidth / 2)*10-5, GridManager.Instance.gridWidth*10, (float)(GridManager.Instance.gridHeight / 4)*10);
    }

    private void OnDisable()
    {
        GameManager.Tiled -= GameManager_Tiled;
    }
   

   
}
