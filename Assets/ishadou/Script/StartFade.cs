using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFade : MonoBehaviour
{
    GameObject canvas;
    GameObject StartObj;
    public GameObject StartText;
    Transform canvasTransform;
    Text textBox;

    float fadeOutTime = 4.0f;
    float blinking = 0f;

    public bool isTurnStart;
    [SerializeField] GameObject backPanel;
    Image backPanelImage;

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        canvasTransform = canvas.GetComponent<Transform>();
        StartObj = (GameObject)Resources.Load("StartText");
        StartText = Instantiate(StartObj, new Vector3(0, 0, 0), Quaternion.identity);
        StartText.transform.SetParent(transform, false);
        textBox = StartText.GetComponent<Text>();
        backPanelImage = backPanel.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void StartFadeOut()
    {

        if (fadeOutTime > 0)
        {
            if (isTurnStart) isTurnStart = false;
            if (fadeOutTime >= 1.8f)
            {
                textBox.text = "Ready";
            }else
            {
                textBox.text = "Start";
            }
            fadeOutTime -= Time.deltaTime;    //制限時間のカウントダウン

            if (fadeOutTime <= 0.5f) //フェードアウト
            {
                blinking -= Time.deltaTime * 2;   //フェードアウト

                textBox.color = new Color(0.7f, 1, 1, blinking);
                backPanelImage.color = new Color(0, 0, 0, blinking / 2);
                textBox.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //拡大
                if (blinking <= 0) blinking = 0;
            }
            else if(fadeOutTime >= 1.3f && fadeOutTime < 1.8f)
            {
                blinking += Time.deltaTime * 2;   //フェードイン

                textBox.color = new Color(0.7f, 1, 1, blinking);
                StartText.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //縮小
                if (blinking >= 1) blinking = 1.0f;
            }
            else if(fadeOutTime <=2.3f && fadeOutTime > 1.8f)
            {
                blinking -= Time.deltaTime * 2;   //フェードアウト

                textBox.color = new Color(0.7f, 1, 1, blinking);
                textBox.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //拡大
            }
            else if (fadeOutTime >= 3.5f )   //秒以下で点滅フェードイン
            {
                blinking += Time.deltaTime * 2;   //フェードイン

                textBox.color = new Color(0.7f, 1, 1, blinking);
                backPanelImage.color = new Color(0, 0, 0, blinking / 2);
                StartText.transform.localScale = new Vector3(1.5f - (blinking / 2), 1.5f - (blinking / 2), 1); //縮小
                if (blinking >= 1) blinking = 1.0f;
            }
        }
        else if (fadeOutTime <= 0)
        {
            //この処理を呼ぶフラグを消す
            fadeOutTime = 2.0f;
            if (!isTurnStart) isTurnStart = true;
        }
    }
}
