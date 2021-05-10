using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thinking : MonoBehaviour
{
    Text thinkingText;
    [SerializeField] Text fadeText;
    public float thinkingTime;
    float blinking;
    float blinkingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        thinkingText = GetComponent<Text>();
        blinking = 0f;
        blinkingSpeed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        blinking = Mathf.Sin(Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        fadeText.color = new Color(0.7f, 1, 1, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
    }

    public void ThinkingTime()
    {
        thinkingTime -= Time.deltaTime;
        //if (thinkingTime <= 0) 

        thinkingText.text = thinkingTime.ToString("f1");
    }
}
