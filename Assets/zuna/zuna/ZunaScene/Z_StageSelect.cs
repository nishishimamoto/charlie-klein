using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Z_StageSelect : MonoBehaviour
{
    public static int cursol = 0;
    int oldCursol;
    bool isVertical;
    bool isHorizontal;
    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    bool isBlinking;
    bool isBack;

    [SerializeField] GameObject[] stage;
    [SerializeField] AudioSource audiosource;
    [SerializeField] GameSE gameSECS;
    // Start is called before the first frame update
    void Start()
    {
        //各ステージの情報リセット
        Isha_SinglshotReSet();
        test3ReSet();
        Test2ReSet();
        stage[cursol].GetComponent<Image>().color = new Color(1, 1, 0, 1f);
        stage[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBlinking)
        {
            //十字キーのパネル選択
            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                oldCursol = cursol;
                if (cursol >= 8) cursol -= 8;
                else cursol += 2;
                //if (cursol >= 2) cursol -= 2;
                //else cursol += 1;
                isVertical = true;
                ButtonSize();
                audiosource.PlayOneShot(gameSECS.cursorSE);
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                oldCursol = cursol;
                if (cursol <= 1) cursol += 8;
                else cursol -= 2;
                //if (cursol <= 0) cursol += 2;
                //else cursol -= 1;
                isVertical = true;
                ButtonSize();
                audiosource.PlayOneShot(gameSECS.cursorSE);
            }
            if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)    //↓入力時
            {
                oldCursol = cursol;
                if (cursol % 2 == 1) cursol -= 1;
                else cursol += 1;
                isHorizontal = true;
                ButtonSize();
                audiosource.PlayOneShot(gameSECS.cursorSE);
            }
            if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)  //↑入力時
            {
                oldCursol = cursol;
                if (cursol % 2 == 0) cursol += 1;
                else cursol -= 1;
                isHorizontal = true;
                ButtonSize();
                audiosource.PlayOneShot(gameSECS.cursorSE);
            }
        }

        if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;
        if (0 == Input.GetAxis("ClossHorizontal") && !isBlinking) isHorizontal = false;

        if (Input.GetButtonDown("A") && !isBlinking && !isBack)
        {
            isBlinking = true;
            isVertical = true;
            audiosource.PlayOneShot(gameSECS.crickSE);
            Invoke("SenceChange", 1.0f);
        }
        if (Input.GetButtonDown("B") && !isBlinking && !isBack)
        {
            isBack = true;
            audiosource.PlayOneShot(gameSECS.crickSE);
            Invoke("StageSelectBack", 1.0f);
        }

        if (isBlinking) Blinking();


        GetComponent<RectTransform>().anchoredPosition
                = new Vector2(-150 + ((cursol % 2) * 300), 130 + ((cursol / 2) * -70));

        //GetComponent<RectTransform>().anchoredPosition
        //= new Vector2(0, 100 + (cursol * -100));
    }

    void Blinking()
    {
        blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        stage[cursol].GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
    }

    void SenceChange()
    {
        SceneManager.LoadScene(20);
        //switch (cursol)
        //{
        //    case 0:
        //        SceneManager.LoadScene("Z_1");
        //        break;
        //    case 1:
        //        SceneManager.LoadScene("Z_2");
        //        break;
        //    case 2:
        //        SceneManager.LoadScene("Z_3");
        //        break;
        //    case 3:
        //        SceneManager.LoadScene("Z_4");
        //        break;
        //    case 4:
        //        SceneManager.LoadScene("Z_5");
        //        break;
        //    case 5:
        //        SceneManager.LoadScene("Z_6");
        //        break;
        //    case 6:
        //        SceneManager.LoadScene("Z_7");
        //        break;
        //    case 7:
        //        SceneManager.LoadScene("Z_8");
        //        break;
        //    case 8:
        //        SceneManager.LoadScene("Z_9");
        //        break;
        //    case 9:
        //        SceneManager.LoadScene("Z_10");
        //        break;
    //}
    }

    void ButtonSize()
    {
        stage[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
        stage[cursol].GetComponent<Image>().color = new Color(1, 1, 0, 1f);
        stage[oldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
        stage[oldCursol].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
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

    void StageSelectBack()
    {
        SceneManager.LoadScene("StageSelect");
    }
}