using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAnim : MonoBehaviour
{
    Animator anim;
    public bool animFlg;

    // Start is called before the first frame update
    void Start()
    {
        // アニメーター（アニメーション制御のやつ）を受け取る
        anim = GetComponent("Animator") as Animator;
    }

    // Update is called once per frame
    void Update()
    {
        if (animFlg)
        {
            //GetComponent<Animator>().SetTrigger("rotation");
            anim.SetBool("rotation", true);
        }else if (!animFlg)
        {
            anim.SetBool("rotation", false);
        }
    }
}
