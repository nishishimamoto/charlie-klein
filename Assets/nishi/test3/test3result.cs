using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test3result : MonoBehaviour
{
    int cursol = 0;
    int oldCursol;
    bool isVertical;
    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    bool isBlinking;

    [SerializeField] GameObject[] button;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //十字キーのパネル選択
        if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
        {
            oldCursol = cursol;
            if (cursol == 2) cursol -= 2;
            else cursol += 2;
            isVertical = true;
            ButtonSize();
        }
        else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
        {
            oldCursol = cursol;
            if (cursol == 0) cursol += 2;
            else cursol -= 2;
            isVertical = true;
            ButtonSize();
        }

        if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;

        if (Input.GetButtonDown("A"))
        {
            isBlinking = true;
            isVertical = true;
            Invoke("SenceChange", 1.0f);
        }

        if (isBlinking) Blinking();


        GetComponent<RectTransform>().anchoredPosition
                = new Vector2(0, -20 + (cursol * -60));
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
                SceneManager.LoadScene("test3");
                break;
            case 1:
                SceneManager.LoadScene("StageSelect");
                break;
            case 2:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
                break;
        }
    }

    void ButtonSize()
    {
        button[cursol].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
        button[oldCursol].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
    }
}
