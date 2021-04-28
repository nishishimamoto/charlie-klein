using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn1 : MonoBehaviour
{
    Text turnText;

    public int nowTurn = 0;
    //public int limitTurn;   //ここにターンの指定をステージごとにいれる
    GameObject canvas;
    GameObject TurnStartObj;
    public GameObject turnStart;
    Transform canvasTransform;
    Text turnStartText;

    float fadeOutTime = 2.0f;
    float blinking = 0f;
    float blinkingSpeed = 1.0f;
    public bool isTurnStart;

    private void Start()
    {
        turnText = GetComponent<Text>();
        turnText.text = "" + (test2.TurnMax - nowTurn);


        canvas = GameObject.Find("Canvas");
        canvasTransform = canvas.GetComponent<Transform>();
        TurnStartObj = (GameObject)Resources.Load("TurnStart");
        turnStart = Instantiate(TurnStartObj, new Vector3(0, 0, 0), Quaternion.identity);
        turnStart.transform.SetParent(canvasTransform, false);
        turnStartText = turnStart.GetComponent<Text>();
    }

    private void Update()
    {

    }

    public void TurnCount()
    {
        nowTurn++;
        if (nowTurn < (test2.TurnMax+1)) turnText.text = "" + (test2.TurnMax - nowTurn);
        Debug.Log(nowTurn);
    }

    public void TurnStart()
    {

        if (fadeOutTime > 0)
        {
            if (isTurnStart) isTurnStart = false;
            turnStartText.text = "turn " + nowTurn + "/5";
            fadeOutTime -= Time.deltaTime;    //制限時間のカウントダウン

            if (fadeOutTime <= 0.5f) //フェードアウト
            {
                blinking -= Time.deltaTime * 2;   //フェードアウト

                turnStartText.color = new Color(0, 255, 255, blinking);
                turnStart.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //拡大
                if (blinking <= 0) blinking = 0;
            }
            else if (fadeOutTime >= 1.5f)   //秒以下で点滅フェードイン
            {
                blinking += Time.deltaTime * 2;   //フェードイン

                turnStartText.color = new Color(0, 255, 255, blinking);
                turnStart.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //縮小
                if (blinking >= 1) blinking = 1.0f;
            }
        }
        else if (fadeOutTime <= 0)
        {
            //この処理を呼ぶフラグを消す
            fadeOutTime = 2.0f;
            if (!isTurnStart) isTurnStart = true;
        }
    }
}
