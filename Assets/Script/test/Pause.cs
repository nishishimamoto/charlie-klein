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

    //[SerializeField] Text pauseText;
    [SerializeField] GameObject buttons;   //全てのポーズUIを子にもつ
    [SerializeField] GameObject[] button = new GameObject[buttonNum];  //カーソル選択時にサイズを変えるUI
    [SerializeField] GameObject pauseSelect;

    [SerializeField] GameObject reallyEndbuttons;   //全てのポーズUIを子にもつ
    [SerializeField] GameObject[] reallyEndButton;
    [SerializeField] GameObject reallyEndSelect;

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

                if (0 == Input.GetAxis("ClossVertical")) isVertical = false;

                if (Input.GetButtonDown("X"))
                {
                    SenceChange();
                }

                //選択している画像を動かす処理
                pauseSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40 + (cursol * -60));

            }
            else if (!isPause)  //ポーズ解除に伴っていろいろ消す
            {
                buttons.SetActive(false);   //様々なポーズUIを非表示
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

            if (0 == Input.GetAxis("ClossVertical")) isVertical = false;

            if (Input.GetButtonDown("X"))
            {
                ReallyEnd();
            }

            //選択している画像を動かす処理
            reallyEndSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40 + (Endcursol * -80));
        }
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
        button[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
        button[oldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
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
        reallyEndButton[Endcursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
        reallyEndButton[EndoldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
    }

    void ReallyEnd()
    {
        switch (Endcursol)
        {
            case 0: //再開
                Invoke("DelayReallyEnd", 0.1f);
                break;
            case 1: //ステージ初めから
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
                break;
        }
    }
}
