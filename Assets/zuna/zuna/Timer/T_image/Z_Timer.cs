using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Z_Timer : MonoBehaviour
{

    public float maxTime = 30.0f;

    Text timerText;
    public float timeCount;            //制限時間
    public bool timeOut;
    public bool countStart;

    [SerializeField] Image timerSlider;
    [SerializeField] Image timerBase;
    [SerializeField] public Text bigTimerText;
    [SerializeField] Turn1 TurnCS;

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
        timerBase.fillAmount = 1.0f - timerSlider.fillAmount;
        ChangeSliderColor();    //15,5秒で円画像の色変更

        if (timeCount > 0 && countStart)
        {
            timeCount -= Time.deltaTime;    //制限時間のカウントダウン
            if (TurnCS.nowTurn >= (test2.TurnMax-5))
            {
                bigTimerText.enabled = true;   //5秒前から表示
                bigTimerText.text = "" + (test2.TurnMax - TurnCS.nowTurn); //時間の表示
                if(TurnCS.nowTurn>=test2.TurnMax) bigTimerText.text = "0";
            }
            else if (TurnCS.nowTurn <= (test2.TurnMax - 5))
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
        else if (timeCount <= 15 && timerSlider.color == Color.white) timerSlider.color = Color.yellow;
        else if (timeCount >= 15 && timerSlider.color != Color.white) timerSlider.color = new Color(0.0f, 1.0f, 1.0f, 0.6f);
    }
}