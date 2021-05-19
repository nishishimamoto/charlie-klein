using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking : MonoBehaviour
{
    Text selfText;
    float blinking = 1;
    float blinkingSpeed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        selfText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        blinking = Mathf.Sin(Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        selfText.color = new Color(1, 1, 1, 0.25f + Mathf.Abs((blinking * 2) / 3));  //絶対値でsin波を透明度に 点滅
    }
}
