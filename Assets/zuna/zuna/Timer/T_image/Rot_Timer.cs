using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot_Timer : MonoBehaviour
{
    public float xxx;
    public float yyy;
    public float speed;

    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // ワールド座標基準で、現在の回転量へ加算する
        myTransform.Rotate(xxx, yyy, speed);
    }
}