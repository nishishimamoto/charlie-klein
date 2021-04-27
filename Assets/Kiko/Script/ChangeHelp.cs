using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加↓
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeHelp : MonoBehaviour
{
    // eventSystemを取得するための変数宣言
    [SerializeField] EventSystem eventSystem;

    //画像差し替え用変数
    public Image image;
    private Sprite sprite;


    //各ステージのタグ格納変数
    [SerializeField] GameObject TYPE_A;
    [SerializeField] GameObject TYPE_B;
    [SerializeField] GameObject TYPE_C;




    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //カーソルが
        // TYPE_A選択中の間Help_Aを表示する
        if (eventSystem.currentSelectedGameObject.gameObject == TYPE_A)
        {
            sprite = Resources.Load<Sprite>("Help_A");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }

        // TYPE_B選択中の間Help_Bを表示する
        if (eventSystem.currentSelectedGameObject.gameObject == TYPE_B)
        {
            sprite = Resources.Load<Sprite>("Help_B");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }

        // TYPE_A選択中の間Help_Aを表示する
        if (eventSystem.currentSelectedGameObject.gameObject == TYPE_C)
        {
            sprite = Resources.Load<Sprite>("Help_C");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }
    }
}