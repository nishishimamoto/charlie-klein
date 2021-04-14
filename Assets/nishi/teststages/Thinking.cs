using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thinking : MonoBehaviour
{
    Text thinkingText;
    public float thinkingTime;
    // Start is called before the first frame update
    void Start()
    {
        thinkingText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        //comboText.color = new Color(255, 255, 255, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
    }

    public void ThinkingTime()
    {
        thinkingTime -= Time.deltaTime;
        //if (thinkingTime <= 0) 

        thinkingText.text = thinkingTime.ToString("f1");
    }
}
