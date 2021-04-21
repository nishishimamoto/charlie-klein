using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotate : MonoBehaviour
{
    private Transform trs;
    private Component @object;
    public float fl;

    Vector3 vec3;
    bool rot_Y;
    // Start is called before the first frame update
    void Start()
    {
        @object = this.GetComponent("center");
    }

    // Update is called once per frame
    void Update()
    {
         trs = this.transform;

        if (Input.GetButtonDown("RB"))
        {
            if (!rot_Y)
            {
                rot_Y = true;
                fl *= -1;
            }
        }

        else if (Input.GetButtonDown("LB"))
        {

            if (rot_Y)
            {
                rot_Y = false;
                fl *= -1;
            }
        }
        
        trs.Rotate(0, 0, fl);

       
    }
}
