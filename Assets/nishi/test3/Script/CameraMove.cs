using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera camera;
    Vector3 center = Vector3.zero;
    Vector3 axis = new Vector3(0,0,-1);
    float speed = 7;

    float cameraTime = 0;
    Vector3 pos;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (camera.enabled)
        {
            transform.RotateAround(center, axis, (360 / speed) * Time.deltaTime);

            cameraTime += Time.deltaTime;
            pos = transform.position;
            if (cameraTime >= 2 && pos.z >= -12) transform.Translate(0, 0, -0.15f);
            else if (cameraTime >= 1.65f && pos.z <= -4) transform.Translate(0, 0, 0.25f);
        }
        if(!camera.enabled && cameraTime > 0)
        {
            cameraTime = 0;
            pos.z = -12;
        }
    }
}
