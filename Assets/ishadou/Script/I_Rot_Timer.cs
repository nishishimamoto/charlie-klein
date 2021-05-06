using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Rot_Timer : MonoBehaviour
{
    [SerializeField] Isha_Singlshot ishaCS;

    public float xxx;
    public float yyy;
    public float speed;

    void Update()
    {
        
        // transformを取得
        Transform myTransform = this.transform;
        if (ishaCS.BonusFlg == 1)
        {
            // ワールド座標基準で、現在の回転量へ加算する
            myTransform.Rotate(xxx, yyy, speed);
        }
        else
        {
            this.transform.rotation = Quaternion.identity;
        }
    }
}
