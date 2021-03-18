using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSelect : MonoBehaviour
{
    int cursol = 0;
    bool ClossTilt;
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
        }
        if (0 < Input.GetAxis("ClossHorizontal") && !ClossTilt)  //↑入力時
        {
            if (cursol <= 0) cursol += 1;
            else cursol -= 1;
            ClossTilt = true;
        }

        if (0 == Input.GetAxis("ClossHorizontal")) ClossTilt = false;

        //ゲーム終了
        if (Input.GetButtonDown("X"))
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


    GetComponent<RectTransform>().anchoredPosition
            = new Vector2(-150 + (cursol * 300), -100);
    }
}
