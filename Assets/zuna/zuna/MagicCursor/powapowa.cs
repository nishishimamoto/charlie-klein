using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powapowa : MonoBehaviour
{

    float alpha=200;
    public int al_Max;
    public int al_Min;
    public float al_Add;
    bool al_fl=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (al_fl)
        {
            alpha += al_Add;
            if(alpha>al_Max)al_fl = false;
        }else if (!al_fl)
        {
            alpha -= al_Add;
            if (alpha < al_Min) al_fl = true;
        }

        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha/255);
    }
}
