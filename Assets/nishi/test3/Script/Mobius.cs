using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobius : MonoBehaviour
{
    Text mobiusText;
    //Color comboColor;

    float fadeOutTime = 2.0f;
    float blinking = 0f;
    float blinkingSpeed = 1.0f;
    float a;

    // Start is called before the first frame update
    void Start()
    {
        mobiusText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutTime > 0)
        {
            fadeOutTime -= Time.deltaTime;    //制限時間のカウントダウン

            if (fadeOutTime >= 1.6f) transform.localScale = new Vector3(3 - ((fadeOutTime - 1) * 2), 3 - ((fadeOutTime - 1) * 2), 1);
            else if (fadeOutTime < 1.6f)   //秒以下で点滅フェードアウト
            {
                blinking = fadeOutTime;   //フェードアウト
                if (fadeOutTime >= 1.0f) a = Mathf.Sin(Mathf.PI * 5 * Time.time);
                else a = 1;
                mobiusText.color = new Color(Mathf.Abs(a), Mathf.Abs(a), 0, blinking / blinkingSpeed);
            }
        }
        else if (fadeOutTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
