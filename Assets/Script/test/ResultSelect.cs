using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSelect : MonoBehaviour
{
    int cursol = 0;
    bool ClossTilt;
    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    bool isBlinking;

    [SerializeField] GameObject retry;
    [SerializeField] GameObject end;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //十字キーのパネル選択
        if (0 > Input.GetAxis("ClossHorizontal") && !ClossTilt)    //↓入力時
        {
            if (cursol >= 1) cursol -= 1;
            else cursol += 1;
            ClossTilt = true;
            ButtonSize();
        }
        if (0 < Input.GetAxis("ClossHorizontal") && !ClossTilt)  //↑入力時
        {
            if (cursol <= 0) cursol += 1;
            else cursol -= 1;
            ClossTilt = true;
            ButtonSize();
        }

        if (0 == Input.GetAxis("ClossHorizontal") && !isBlinking) ClossTilt = false;

        if (Input.GetButtonDown("X"))
        {
            isBlinking = true;
            ClossTilt = true;
            Invoke("SenceChange", 1.0f);
        }

        if (isBlinking) Blinking();


    GetComponent<RectTransform>().anchoredPosition
            = new Vector2(-150 + (cursol * 300), -100);
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
                    SceneManager.LoadScene("Stage3");
                    break;
                case 1:
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
        switch (cursol)
        {
            case 0:
                retry.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
                end.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
                break;
            case 1:
                end.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
                retry.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
                break;
        }
    }
}
