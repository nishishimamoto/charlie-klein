using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotate : MonoBehaviour
{
    private Transform trs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         trs = this.transform;

        trs.Rotate(0, 0, 0.2f);
    }
}
