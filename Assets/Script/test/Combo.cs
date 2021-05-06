using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    Text comboText;
    //Color comboColor;

    public float comboTime = 0f;
    public int comboCount = 0;
    float blinking = 0f;
    float blinkingSpeed = 2.0f;
    public bool isMobius;
    GameObject combo; //コンボの元
    GameObject mobius;  //全消しでメビウスをだす
    [SerializeField] GameObject canvas;
    Transform canvasTransform;//キャンバスではなく空オブジェに変更
    // Start is called before the first frame update
    void Start()
    {
        comboText = GetComponent<Text>();
        comboText.color = new Color(255, 255, 255, 0);  //コンボ表記透明

        combo = (GameObject)Resources.Load("ComboCount");
        mobius = (GameObject)Resources.Load("Mobius");
        //canvas = GameObject.Find("center");
        canvasTransform = canvas.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTime > 0)
        {
            comboTime -= Time.deltaTime;    //制限時間のカウントダウン
            comboText.text = comboCount + "Combo!";

            if (comboTime >= 2) comboText.color = new Color(255, 255, 255, 1.0f);
            else if (comboTime < 2)   //2秒以下で点滅
            {
                //blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
                //comboText.color = new Color(255, 255, 255, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅

                blinking = comboTime;   //フェードアウト
                comboText.color = new Color(255, 255, 255, blinking / 2);
            }
        }
        else if (comboTime <= 0)
        {
            comboCount = 0;
            comboText.color = new Color(255, 255, 255, 0);  //コンボ表記透明
        }

        if (isMobius) Mobius();
    }

    public void BoardCombo(int x)   //盤面に〇コンボ!と出す
    {
        GameObject combos = Instantiate(combo, new Vector3(-280 + (95 * (x % 6)), 185 + (-95 * (x / 6)), 0), Quaternion.identity);
        combos.transform.SetParent(canvasTransform, false);
        Text combosText = combos.GetComponent<Text>();
        combosText.text = comboCount + "";
    }

    public void Mobius()
    {
        GameObject combos = Instantiate(mobius, new Vector3(0, 0, 0), Quaternion.identity);
        combos.transform.SetParent(canvasTransform, false);
        isMobius = false;
    }
}
