using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加↓
using UnityEngine.UI;

public class ChangeHelp : MonoBehaviour
{

    public Image image;
    private Sprite sprite;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //カーソルが
        // TYPE_A選択中の間Help_Aを表示する
        if (Input.GetKeyDown(KeyCode.Z))
        {
            sprite = Resources.Load<Sprite>("Help_A");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }

        // TYPE_B選択中の間Help_Bを表示する
        if (Input.GetKeyDown(KeyCode.Y))
        {
            sprite = Resources.Load<Sprite>("Help_B");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }

        // TYPE_A選択中の間Help_Aを表示する
        if (Input.GetKeyDown(KeyCode.X))
        {
            sprite = Resources.Load<Sprite>("Help_C");
            image = this.GetComponent<Image>();
            image.sprite = sprite;
        }
    }
}