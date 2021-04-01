using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboFadeOut : MonoBehaviour
{
    Text comboText;
    //Color comboColor;

    float fadeOutTime = 1.0f;
    float blinking = 0f;
    float blinkingSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        comboText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutTime > 0)
        {
            fadeOutTime -= Time.deltaTime;    //制限時間のカウントダウン

            if (fadeOutTime >= 0.6f) transform.localScale = new Vector3(3 - (fadeOutTime * 2), 3 - (fadeOutTime * 2), 1);
            else if (fadeOutTime < 0.6f)   //秒以下で点滅フェードアウト
            {
                blinking = fadeOutTime;   //フェードアウト

                comboText.color = new Color(255, 255, 0, blinking / blinkingSpeed);
            }
        }
        else if (fadeOutTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
