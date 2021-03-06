using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip tipsSE;
    [SerializeField] AudioClip clickSE;

    [SerializeField] GameObject[] modeTips;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject loadImage;
    GameObject []tips;
    SpriteRenderer[] tipsRenderer;
    [SerializeField] Text[] tipsNumber;
    [SerializeField] SpriteRenderer fadeSprite;
    bool isFade;
    float fadeValue = 1;
    int nowTipsCursol = 0;
    int oldTipsCursol = 1;
    float sceneTime;
    bool isHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        tips = new GameObject[modeTips[StageSelect.cursol].transform.childCount];
        tipsRenderer = new SpriteRenderer[modeTips[StageSelect.cursol].transform.childCount];
        TipsGet();
        TipsNumber();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFade) TipsChange();
        SceneChange();
        TipsActive();
        Fade(); //フェードイン、アウトの処理
        //if (Input.GetButtonDown("Y"))
        //{
        //    if (isFade) isFade = false;
        //    else if (!isFade) isFade = true;
        //}
    }

    void TipsGet()
    {
        if(!modeTips[StageSelect.cursol].activeSelf) modeTips[StageSelect.cursol].SetActive(true);

        for (int i = 0; i < modeTips[StageSelect.cursol].transform.childCount; i++)
        {
            tips[i] = modeTips[StageSelect.cursol].transform.GetChild(i).gameObject;
            tips[i].SetActive(true);
            tipsRenderer[i] = tips[i].GetComponent<SpriteRenderer>();
        }
    }

    void SceneChange()
    {
        if (sceneTime < 3)
        {
            sceneTime += Time.deltaTime;
        }
        else if (sceneTime >= 3)
        {
            if(!nextButton.activeSelf) nextButton.SetActive(true);
            if (loadImage.activeSelf) loadImage.SetActive(false);

            if (Input.GetButtonDown("A") && !isFade)
            {
                audioSource.PlayOneShot(clickSE);
                isFade = true;
                Invoke("DelayChange",1);
            }
        }
    }

    void DelayChange()
    {
        if (StageSelect.cursol == 1) SceneManager.LoadScene(4); //make
        else if (StageSelect.cursol == 0) SceneManager.LoadScene(5); //satellite
        else SceneManager.LoadScene(9 + Z_StageSelect.cursol);  //
    }

    void Fade()
    {
        if (!isFade && fadeValue >= 0)
        {
            fadeValue -= Time.deltaTime * 3;
            fadeSprite.color = new Color(0, 0, 0, fadeValue);
        }
        else if (isFade && fadeValue <= 1)
        {
            fadeValue += Time.deltaTime * 3;
            fadeSprite.color = new Color(0, 0, 0, fadeValue);
        }
    }

    void TipsChange()
    {
        if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)  //←入力時
        {
            oldTipsCursol = nowTipsCursol;
            if (nowTipsCursol % 2 == 1) nowTipsCursol -= 1;
            else nowTipsCursol += 1;
            isHorizontal = true;
            audioSource.PlayOneShot(tipsSE);
            TipsNumber();
        }
        if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)    //→入力時
        {
            oldTipsCursol = nowTipsCursol;
            if (nowTipsCursol % 2 == 0) nowTipsCursol += 1;
            else nowTipsCursol -= 1;
            isHorizontal = true;
            audioSource.PlayOneShot(tipsSE);
            TipsNumber();
        }

        if (0 == Input.GetAxis("ClossHorizontal")) isHorizontal = false;
    }

    void TipsNumber()
    {
            tipsNumber[0].text = "" + (nowTipsCursol + 1);
            tipsNumber[1].text = "" + (modeTips[StageSelect.cursol].transform.childCount);
    }

    void TipsActive()
    {
        if (!tipsRenderer[nowTipsCursol].enabled) tipsRenderer[nowTipsCursol].enabled = true;
        if (tipsRenderer[oldTipsCursol].enabled) tipsRenderer[oldTipsCursol].enabled = false;
    }
}
