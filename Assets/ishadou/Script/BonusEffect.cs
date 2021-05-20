using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusEffect : MonoBehaviour
{
    GameObject canvas;
    Transform canvasTransform;
    Text FadeText;

    [SerializeField] Isha_Singlshot ishaCS;
    [SerializeField] Image[] _mgs;
    [SerializeField] Image[] _borders;
    [SerializeField] Image BonusTextBack;
    [SerializeField] Text CntText;
    [SerializeField] float _fadeTime = 0.5f;
    [SerializeField] float _textFadeTime = 0.5f;
    [SerializeField] float _mgsFadeStartTime = 0f;
    [SerializeField] float _bordersFadeStartTime = 0.5f;
    [SerializeField] string[] CntTextLog;

    float nowFadeTime;
    private Color[] mgsFixedColor = new Color[2];
    private Color[] bdFixedColor = new Color[3];
    private Color[] mgsStartColor = new Color[2];
    private Color[] bordersStartColor = new Color[3];
    private Color textStartColor;

    void Start()
    {
        int i = 0;
        foreach (var mg in _mgs)
        {
            mg.gameObject.SetActive(false);
            mgsFixedColor[i] = mg.color;
            i++;
        }

        i = 0;
        foreach (var bd in _borders)
        {
            bd.gameObject.SetActive(false);
            bdFixedColor[i] = bd.color;
            i++;
        }
        textStartColor = CntText.color;
        BonusTextBack.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void FadeStart()
    {
        int i = 0;

        foreach (var mg in _mgs)
        {
            mg.gameObject.SetActive(true);
            mgsStartColor[i] = mg.color;
            mg.color = new Color(0, 0, 0, 0);
            i++;
        }

        i = 0;
        foreach (var bd in _borders)
        {
            bd.gameObject.SetActive(true);
            bordersStartColor[i] = bd.color;
            bd.color = new Color(0, 0, 0, 0);
            i++;
        }

        ishaCS.isCut = true;

        StartCoroutine(nameof(ImageFade));
    }
    private IEnumerator ImageFade()
    {
        float waitTime = _fadeTime;
        nowFadeTime = 0;
        Color endColor = new Color(0, 0, 0, 0);

        // あらわれたい
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            int i = 0;
            float rate = nowFadeTime / 0.3f;
            foreach(var mg in _mgs)
            {
                mg.color = Color.Lerp(endColor, mgsStartColor[i], rate);
                i++;
            }
            yield return new WaitForFixedUpdate();
        }

        // きえて、あらわれたい
        nowFadeTime = 0;
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            int i = 0;
            float rate = nowFadeTime / 0.3f;
            foreach (var mg in _mgs)
            {
                mg.color = Color.Lerp(mgsStartColor[i], endColor, rate);
                i++;
            }

            int j = 0;
            foreach (var bd in _borders)
            {
                bd.color = Color.Lerp(endColor, bordersStartColor[j], rate);
                j++;
            }
            yield return new WaitForFixedUpdate();
        }

        //ボーナス開始のカウント
        waitTime = _textFadeTime;
        int k = CntTextLog.Length - 1;
        BonusTextBack.gameObject.SetActive(true);
        while (k > -1)
        {
            nowFadeTime = 0;
            while (nowFadeTime < waitTime)
            {
                nowFadeTime += Time.deltaTime;
                CntText.text = CntTextLog[k];
                float rate = nowFadeTime / 0.2f;
                CntText.gameObject.SetActive(true);
                yield return new WaitForFixedUpdate();
            }
            k--;
        }

        BonusTextBack.gameObject.SetActive(false);
        CntText.gameObject.SetActive(false);
        //↓ 0.5秒の待ち
        nowFadeTime = _fadeTime;
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        //BonusTextBack.gameObject.SetActive(false);
        ishaCS.BonusFlg = 1;
        ishaCS.isCut = false;
    }

    public void EndFade()
    {
        int i = 0;
        foreach (var mg in _mgs)
        {
            mg.gameObject.SetActive(false);
            mgsStartColor[i] = mg.color;
            mg.color = mgsFixedColor[i];
            i++;
        }

        i = 0;
        foreach (var bd in _borders)
        {
            bd.gameObject.SetActive(false);
            bordersStartColor[i] = bd.color;
            bd.color = bdFixedColor[i];
            i++;
        }
        StartCoroutine(nameof(EndImageFade));
    }

    private IEnumerator EndImageFade()
    {
        ishaCS.isEndFadeWait = true;

        float waitTime = 0.2f;
        nowFadeTime = 0;
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        waitTime = _textFadeTime;
        nowFadeTime = 0;
        BonusTextBack.gameObject.SetActive(true);
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            CntText.text = "Time Out";
            CntText.gameObject.SetActive(true);
            yield return new WaitForFixedUpdate();
        }
        CntText.gameObject.SetActive(false);
        BonusTextBack.gameObject.SetActive(false);

        waitTime = 0.1f;
        nowFadeTime = 0;
        while (nowFadeTime < waitTime)
        {
            nowFadeTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("ant");

        ishaCS.BonusFlg = 0;
        ishaCS.isEndFadeWait = false;
    }
}

