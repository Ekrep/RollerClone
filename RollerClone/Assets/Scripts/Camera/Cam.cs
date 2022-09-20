using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]private float _distanceBetweenMapAndCam;
    void Start()
    {
        //this.gameObject.transform.position = new Vector3(GridManager.Instance.gridWidth / 2, _distanceBetweenMapAndCam, GridManager.Instance.gridHeight / 2);   
    }

   
}
