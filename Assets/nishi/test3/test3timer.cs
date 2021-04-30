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
    float bigTimerBlinking = 1;

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
<<<<<<< HEAD

=======
>>>>>>> desktop
                bigTimerBlinking -= Time.deltaTime;
                if (bigTimerBlinking <= 0) bigTimerBlinking = 1;
                else if (bigTimerBlinking <= 0.4)
                {
                    bigTimerText.color = new Color(255, 255, 255, bigTimerBlinking);
                    bigTimerText.transform.localScale = new Vector3(1.4f - bigTimerBlinking, 1.4f - bigTimerBlinking, 1);
                }
                if (bigTimerBlinking == 1)
                {
                    bigTimerText.color = new Color(255, 255, 255, 0.4f);
                    bigTimerText.transform.localScale = new Vector3(1, 1, 1);
                }
                bigTimerText.text = Mathf.Ceil(timeCount).ToString("f0");  //時間の表示
            }
            else if (timeCount > 5)
            {
                bigTimerText.enabled = false;
                bigTimerBlinking = 1;
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
        if (timeCount <= 5 && timerSlider.color != Color.red) timerSlider.color = Color.red;
        else if (timeCount <= 10 && timeCount > 5 && timerSlider.color != Color.yellow) timerSlider.color = Color.yellow;
        else if (timeCount > 10 && timerSlider.color != new Color32(0, 255, 255, 170)) timerSlider.color = new Color32(0, 255, 255, 170);
    }
}
