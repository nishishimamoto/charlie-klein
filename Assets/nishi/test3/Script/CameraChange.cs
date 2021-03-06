using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera subCamera;
    public bool isParfectCamera;


    void Start()
    {

    }

    void Update()
    {
        if (isParfectCamera)
        {
            mainCamera.enabled = false;
            subCamera.enabled = true;
        }
        else if (!isParfectCamera)
        {
            subCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }
}
