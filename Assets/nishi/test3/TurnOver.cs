using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOver : MonoBehaviour
{
    const int maxlife = 4;
    public int lifeSpan;
    // Start is called before the first frame update
    void Start()
    {
        lifeSpan = maxlife;
        GetComponent<TextMesh>().text = "" + lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Y")) LifeCountDown();
        //if (Input.GetButtonDown("X")) LifeCountReSet();
    }

    public void LifeCountDown()
    {
        lifeSpan -= 1;

        if(lifeSpan == 2) GetComponent<TextMesh>().color = Color.yellow;//黄
        else if (lifeSpan == 1) GetComponent<TextMesh>().color = Color.red;//赤

        GetComponent<TextMesh>().text = "" + lifeSpan;
    }

    public void LifeCountReSet()
    {
        lifeSpan = maxlife;
        GetComponent<TextMesh>().color = Color.white;//白
        GetComponent<TextMesh>().text = "" + lifeSpan;
    }
}
