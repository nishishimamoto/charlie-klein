using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Z_Limit : MonoBehaviour
{
    Text limitText;
    bool f1,f2;
    // Start is called before the first frame update
    void Start()
    {
        limitText = this.GetComponent<Text>();
        if (test2.TurnMax < 10) f1 = true;
        if (test2.TurnMax >= 10 && test2.TurnMax < 20) f2 = true;
    }

    // Update is called once per frame
    void Update()
    {
        limitText.text = "/" + test2.TurnMax;
        if (f1) limitText.text = "/" + test2.TurnMax + "\u00A0\u00A0";
        if (f2) limitText.text = "/" + test2.TurnMax + "\u00A0";
    }
}
