using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    static int buttonNum = 4;   //再開、リトライ、ステージ選択、終了の4

    public bool isPause;
    bool isReallyEnd;   //本当に終了するか聞くやつ

    int cursol = 0;
    int oldCursol;

    int Endcursol = 0;
    int EndoldCursol;

    bool isVertical;

    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    float blinkingTime = 0;
    bool isBlinking;

    //[SerializeField] Text pauseText;
    [SerializeField] GameObject buttons;   //全てのポーズUIを子にもつ
    [SerializeField] GameObject[] button = new GameObject[buttonNum];  //カーソル選択時にサイズを変えるUI
    [SerializeField] GameObject pauseSelect;

    [SerializeField] GameObject reallyEndbuttons;   //全てのポーズUIを子にもつ
    [SerializeField] GameObject[] reallyEndButton;
    [SerializeField] GameObject reallyEndSelect;
    [SerializeField] GameObject ScreenCover;
    [SerializeField] GameObject[] cursor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isReallyEnd)
        {
            reallyEndbuttons.SetActive(false);   //本当に終了しますか？を非表示

            //ポーズ
            if (Input.GetButtonDown("Start"))   //Yを押したら終了
            {
                if (isPause) isPause = false;
                else if (!isPause) isPause = true;
            }

            if (isPause)
            {
                buttons.SetActive(true);    //様々なポーズUIを表示
                ScreenCover.SetActive(true);

                //十字キーのパネル選択
                if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
                {
                    oldCursol = cursol;
                    if (cursol == (buttonNum - 1)) cursol -= (buttonNum - 1);
                    else cursol += 1;
                    isVertical = true;
                    ButtonSize();
                }
                else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
                {
                    oldCursol = cursol;
                    if (cursol == 0) cursol += (buttonNum - 1);
                    else cursol -= 1;
                    isVertical = true;
                    ButtonSize();
                }

                if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;

                if (Input.GetButtonDown("A"))
                {
                    isBlinking = true;
                    isVertical = true;
                    //SenceChange();
                    Invoke("SenceChange", 1.0f);
                }

                if (Input.GetButtonDown("B") && cursol != 0)
                {
                    oldCursol = cursol;
                    cursol = 0;
                    ButtonSize();
                }
                if (isBlinking) Blinking();

                //選択している画像を動かす処理
                pauseSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40 + (cursol * -80));

            }
            else if (!isPause)  //ポーズ解除に伴っていろいろ消す
            {
                buttons.SetActive(false);   //様々なポーズUIを非表示
                ScreenCover.SetActive(false);
            }
        }
        else if (isReallyEnd)
        {
            buttons.SetActive(false);   //様々なポーズUIを非表示
            reallyEndbuttons.SetActive(true);   //本当に終了しますか？を表示

            //十字キーのパネル選択
            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                EndoldCursol = Endcursol;
                if (Endcursol == 1) Endcursol -= 1;
                else Endcursol += 1;
                isVertical = true;
                ReallyEndButtonSize();
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                EndoldCursol = Endcursol;
                if (Endcursol == 0) Endcursol += 1;
                else Endcursol -= 1;
                isVertical = true;
                ReallyEndButtonSize();
            }

            if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;

            if (Input.GetButtonDown("A"))
            {
                isBlinking = true;
                isVertical = true;
                Invoke("ReallyEnd", 1.0f);
                //ReallyEnd();
            }
            if (isBlinking) Blinking();

            //選択している画像を動かす処理
            reallyEndSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40 + (Endcursol * -80));
        }
    }

    void Blinking()
    {
        blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅

        blinkingTime += Time.deltaTime;

        if (blinkingTime >= 1)
        {
            blinkingTime = 0;
            blinking = 1;
            isBlinking = false;
        }

        if (cursor[0].activeSelf) cursor[0].GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
        if(cursor[1].activeSelf) cursor[1].GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
    }

    void SenceChange()
    {
        switch (cursol)
        {
            case 0: //再開
                Invoke("DelayIsPause", 0.1f);
                break;
            case 1: //ステージ初めから
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 2: //ステージセレクト
                SceneManager.LoadScene("StageSelect");
                break;
            case 3: //ゲーム終了(Yes or No)聞く
                isReallyEnd = true;
                break;
        }
    }

    void ButtonSize()
    {
        button[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1);
        button[oldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1);
    }

    void DelayIsPause()
    {
        isPause = false;
    }

    void DelayReallyEnd()
    {
        isReallyEnd = false;
    }

    void ReallyEndButtonSize()
    {
        reallyEndButton[Endcursol].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1);
        reallyEndButton[EndoldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1);
    }

    void ReallyEnd()
    {
        switch (Endcursol)
        {
            case 0: //再開
                Invoke("DelayReallyEnd", 0.1f);
                break;
            case 1: //ステージ初めから
                SceneManager.LoadScene("Title");
                break;
        }
    }
}
