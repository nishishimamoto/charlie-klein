using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rot_Cursor : MonoBehaviour
{
    [SerializeField] Image BonusGaugeSand;
    [SerializeField] private Color _fullChargecolor;

    private SpriteRenderer spriteRenderer;
    private Color startColor;

    float alpha = 255;
    public int al_Max;
    public int al_Min;
    public float al_Add;
    bool al_fl = false;

    public float xxx;
    public float yyy;
    public float speed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    void Update()
    {
        if (al_fl)
        {
            alpha += al_Add;
            if (alpha > al_Max) al_fl = false;
        }
        else if (!al_fl)
        {
            alpha -= al_Add;
            if (alpha < al_Min) al_fl = true;
        }

        if (BonusGaugeSand.fillAmount == 1.0f)
        {
            _fullChargecolor.a = alpha / 255;

            spriteRenderer.color = _fullChargecolor;
        }
        else
        {
            Color newColor = startColor;
            newColor.a = alpha / 255;

            spriteRenderer.color = newColor;
            
        }
        // transformを取得
        Transform myTransform = this.transform;

        // ワールド座標基準で、現在の回転量へ加算する
        myTransform.Rotate(xxx, yyy, speed);
    }
}