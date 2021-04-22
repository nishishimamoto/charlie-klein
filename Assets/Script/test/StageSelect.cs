using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    int cursol = 0;
    int oldCursol;
    bool isVertical;
    bool isHorizontal;
    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    bool isBlinking;

    [SerializeField] GameObject[] stage;
    // Start is called before the first frame update
    void Start()
    {
        //各ステージの情報リセット
        Isha_SinglshotReSet();
        test3ReSet();
        Test2ReSet();
    }

    // Update is called once per frame
    void Update()
    {

        //十字キーのパネル選択
        if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
        {
            oldCursol = cursol;
            //if (cursol >= 8) cursol -= 8;
            //else cursol += 2;
            if (cursol >= 2) cursol -= 2;
            else cursol += 1;
            isVertical = true;
            ButtonSize();
        }
        else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
        {
            oldCursol = cursol;
            //if (cursol <= 1) cursol += 8;
            //else cursol -= 2;
            if (cursol <= 0) cursol += 2;
            else cursol -= 1;
            isVertical = true;
            ButtonSize();
        }
        //if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)    //↓入力時
        //{
        //    oldCursol = cursol;
        //    if (cursol % 2 == 1) cursol -= 1;
        //    else cursol += 1;
        //    isHorizontal = true;
        //    ButtonSize();
        //}
        //if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)  //↑入力時
        //{
        //    oldCursol = cursol;
        //    if (cursol % 2 == 0) cursol += 1;
        //    else cursol -= 1;
        //    isHorizontal = true;
        //    ButtonSize();
        //}

        if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;
        if (0 == Input.GetAxis("ClossHorizontal") && !isBlinking) isHorizontal = false;

        if (Input.GetButtonDown("A"))
        {
            isBlinking = true;
            isVertical = true;
            Invoke("SenceChange", 1.0f);
        }

        if (isBlinking) Blinking();


        //GetComponent<RectTransform>().anchoredPosition
        //        = new Vector2(-150 + ((cursol % 2) * 300), 130 + ((cursol / 2) * -70));

        GetComponent<RectTransform>().anchoredPosition
        = new Vector2(0, 100 + (cursol * -100));
    }

    void Blinking()
    {
        blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
    }

    void SenceChange()
    {
        switch (cursol)
        {
            case 0:
                //SceneManager.LoadScene("zuna1");
                SceneManager.LoadScene("Isha_Singlshot");
                break;
            case 1:
                //SceneManager.LoadScene("zuna2");
                SceneManager.LoadScene("Test2");
                break;
            case 2:
                SceneManager.LoadScene("Test3");
                break;
            case 3:
                SceneManager.LoadScene("nishi2");
                break;
            case 4:
                SceneManager.LoadScene("Crescent Moon");
                break;
            case 5:
                SceneManager.LoadScene("Mobius");
                break;
            case 6:
                SceneManager.LoadScene("tatsuki01");
                break;
            case 7:
                SceneManager.LoadScene("tatsuki02");
                break;
            case 8:
                SceneManager.LoadScene("ishadou1");
                break;
            case 9:
                SceneManager.LoadScene("ishadou2");
                break;
        }
    }

    void ButtonSize()
    {
        stage[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
        stage[oldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
    }

    void Isha_SinglshotReSet()
    {
        Isha_Singlshot.score = 0;
        Isha_Singlshot.oldSceneName = null;
    }

    void test3ReSet()
    {
        test3.score = 0;
        test3.oldSceneName = null;
    }

    void Test2ReSet()
    {
        test2.score = 0;
        test2.oldSceneName = null;
    }
}
