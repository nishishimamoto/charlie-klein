using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Z_Limit : MonoBehaviour
{
    Text limitText;
    // Start is called before the first frame update
    void Start()
    {
        limitText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        limitText.text = "/" + test2.TurnMax;
    }
}
