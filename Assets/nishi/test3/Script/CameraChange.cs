using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject subCamera;
    public bool isParfectCamera;


    void Start()
    {

    }

    void Update()
    {
        if (isParfectCamera)
        {
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else if (!isParfectCamera)
        {
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
