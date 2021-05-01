using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加↓
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class ChangeHelp : MonoBehaviour
{

    //画像差し替え用変数
    public Image image;
    private Sprite sprite;

    ////動画差し替え用変数
    public VideoPlayer videoPlayer;
    private VideoClip videoClip;


    // eventSystemを取得するための変数宣言
    [SerializeField] EventSystem eventSystem;

    //前回選択時のgetCursol格納用
    int OldCursol = StageSelect.cursol;

    //StageSelectのCursolを取得する用
    int getCursol;

    // Update is called once per frame
    void Update()
    {
        getCursol = StageSelect.cursol;
       // Debug.Log(getCursol);

        //前回選択時のカーソルと比較
        //中身が違ったら
        if (getCursol != OldCursol)
        {
            //Debug.Log("If_Check");
            HelpVideoChange();
            HelpImageChange();
        }

    }

    void HelpImageChange()
    {

        //今選択中のカーソルが
        switch (getCursol)
        {
            //TYPE_AならHelp_Aを表示する
            case 0:
                sprite = Resources.Load<Sprite>("Help_A");
                image = GameObject.Find("HelpImage").GetComponent<Image>();
                image.sprite = sprite;
                break;
            //TYPE_BならHelp_Bを表示する
            case 1:
                sprite = Resources.Load<Sprite>("Help_B");
                image = GameObject.Find("HelpImage").GetComponent<Image>();
                image.sprite = sprite;
                break;
            //TYPE_CならHelp_Cを表示する
            case 2:
                sprite = Resources.Load<Sprite>("Help_C");
                image = GameObject.Find("HelpImage").GetComponent<Image>();
                image.sprite = sprite;
                break;
        }

        //現在の選択中カーソルを前回選択カーソルに変える
        OldCursol = getCursol;
    }

    void HelpVideoChange()
    {
        //今選択中のカーソルが
        switch (getCursol)
        {
            //TYPE_AならHelp_Aを表示する
            case 0:
                videoClip = Resources.Load<VideoClip>("HelpVideo_A");
                videoPlayer = GameObject.Find("HelpVideo").GetComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                break;
            //TYPE_BならHelp_Bを表示する
            case 1:
                videoClip = Resources.Load<VideoClip>("HelpVideo_B");
                videoPlayer = GameObject.Find("HelpVideo").GetComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                break;
            //TYPE_CならHelp_Cを表示する
            case 2:
                videoClip = Resources.Load<VideoClip>("HelpVideo_C");
                videoPlayer = GameObject.Find("HelpVideo").GetComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                break;
        }
    }
}