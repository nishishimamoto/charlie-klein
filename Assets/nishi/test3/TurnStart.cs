using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStart : MonoBehaviour
{
    //public int limitTurn;   //ここにターンの指定をステージごとにいれる
    GameObject canvas;
    GameObject TurnStartObj;
    public GameObject turnStart;
    Transform canvasTransform;
    Text turnStartText;

    float fadeOutTime = 2.0f;
    float blinking = 0f;
    public bool isTurnStart;

    private void Start()
    {
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

    public void GameStart()
    {

        if (fadeOutTime > 0)
        {
            if (isTurnStart) isTurnStart = false;
            turnStartText.text = "start";
            fadeOutTime -= Time.deltaTime;    //制限時間のカウントダウン

            if (fadeOutTime <= 0.5f) //フェードアウト
            {
                blinking -= Time.deltaTime * 2;   //フェードアウト

                turnStartText.color = new Color(0.7f, 1, 1, blinking);
                turnStart.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //拡大
                if (blinking <= 0) blinking = 0;
            }
            else if (fadeOutTime >= 1.5f)   //秒以下で点滅フェードイン
            {
                blinking += Time.deltaTime * 2;   //フェードイン

                turnStartText.color = new Color(0.7f, 1, 1, blinking);
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
