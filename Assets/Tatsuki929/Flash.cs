using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    Vector3 vec3;
    Transform  trs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trs = this.transform;

        vec3.x = Mathf.Sin(Time.time)/3*2;
        vec3.z = Mathf.Sin(Time.time)/3*2;
        vec3.y = Mathf.Sin(Time.time)/3*2;

        trs.localScale = vec3;
    }
}
