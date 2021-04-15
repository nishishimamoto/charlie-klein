using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test3timer : MonoBehaviour
{
    public float maxTime = 20.0f;

    Text timerText;
    public float timeCount;            //制限時間
    public bool timeOut;
    public bool countStart;

    [SerializeField] Image timerSlider;
    [SerializeField] public Text bigTimerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
        timeCount = maxTime;
        timerText.text = Mathf.Ceil(timeCount).ToString("f0");  //時間の表示
        bigTimerText.enabled = false;   //5秒前から表示
    }

    public void TimerCount()
    {
        timerText.text = Mathf.Ceil(timeCount).ToString("f0");  //時間の表示
        timerSlider.fillAmount = timeCount / maxTime;    //円画像の表示
        ChangeSliderColor();    //15,5秒で円画像の色変更

        if (timeCount > 0 && countStart)
        {
            timeCount -= Time.deltaTime;    //制限時間のカウントダウン
            if (timeCount <= 5)
            {
                bigTimerText.enabled = true;   //5秒前から表示
                bigTimerText.text = Mathf.Ceil(timeCount).ToString("f0");  //時間の表示
            }
            else if (timeCount > 5)
            {
                bigTimerText.enabled = false;
            }
        }
        //else if (timeCount <= 0)
        //{
        //    timeOut = true;
        //    countStart = false;
        //    timeCount = 0;
        //}
    }

    void ChangeSliderColor()
    {
        if (timeCount <= 5 && timerSlider.color == Color.yellow) timerSlider.color = Color.red;
        else if (timeCount <= 10 && timerSlider.color == Color.white) timerSlider.color = Color.yellow;
        else if (timeCount > 10 && timerSlider.color != Color.white) timerSlider.color = Color.white;
    }
}
