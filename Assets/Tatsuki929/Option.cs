using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using UnityEditor;
public class Option : MonoBehaviour
{
    public AudioMixer mixer;

    public float vol_SE, vol_BGM;

    short cursol = 0, cursol_old = 0, cursol2 = 0, cursol_old2 = 0;

    public bool isVertical, isHorizontal, isSound, isGraph, isBack;

    public bool source_SE, source_BGM;

    float blinking = 0f;
    float blinkingSpeed = 3.0f;
    bool isBlinking;

    [SerializeField] GameSE gameSECS;

    [SerializeField] GameObject obj_Sound;
    [SerializeField] GameObject obj_Graphic;
    [SerializeField] GameObject obj_Back;

    [SerializeField] GameObject obj_Cursol;
    [SerializeField] GameObject obj_Cursol2;

    [SerializeField] GameObject vol_Setting;
    [SerializeField] GameObject vol_obj_Back;

    [SerializeField] GameObject vol_obj_SE;
    [SerializeField] GameObject vol_obj_BGM;

    [SerializeField] Text text_BGM, text_SE,text_BGM_num, text_SE_num;
    [SerializeField] GameObject opt_text;

    [SerializeField] SE_Cursol _Cursol;

    [SerializeField] GameObject obj_Gauge_BGM;
    [SerializeField] GameObject obj_Gauge_SE;
    // Start is called before the first frame update
    void Start()
    {
        mixer.GetFloat("SE", out vol_SE);
        mixer.GetFloat("BGM", out vol_BGM);
        
        

    }

    // Update is called once per frame
    void Update()
    {

        if (vol_BGM <= -30)
        {
            
            source_BGM = true;

        }
        if (vol_SE <= -30)
        {
            
            source_SE = true;

        }
        if (!isBack && !isGraph && !isSound)
        {
            obj_Sound.SetActive(true);
            //obj_Graphic.SetActive(true);
            obj_Back.SetActive(true);
            obj_Cursol.SetActive(true);
            vol_Setting.SetActive(false);
            opt_text.SetActive(true);

            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                 cursol_old = cursol;
                //if (cursol >= 8) cursol -= 8;
                //else cursol += 2;
                if (cursol >= 1) cursol -= 1;
                else cursol += 1;
                isVertical = true;
                ButtonSize();
                gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                cursol_old = cursol;
                //if (cursol <= 1) cursol += 8;
                //else cursol -= 2;
                if (cursol <= 0) cursol += 1;
                else cursol -= 1;
                isVertical = true;
                ButtonSize();

                gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
            }
        }
        else if(isSound)
        {
            obj_Sound.SetActive(false);
            //obj_Graphic.SetActive(false);
            obj_Back.SetActive(false);
            obj_Cursol.SetActive(false);
            vol_Setting.SetActive(true);
            opt_text.SetActive(false);

            
            ButtonSize2();
            Slider();

            text_BGM_num.text = Mathf.Floor(((vol_BGM+30)/50)*100).ToString();
            text_SE_num.text = Mathf.Floor(((vol_SE + 30) / 50)*100).ToString();

            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                cursol_old2 = cursol2;
                if (cursol2 >= 2) cursol2 -= 2;
                else cursol2 += 1;
                isVertical = true;
                gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                //ButtonSize2();
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                cursol_old2 = cursol2;

                if (cursol2 <= 0) cursol2 += 2;
                else cursol2 -= 1;
                isVertical = true;
                gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                //ButtonSize2();
            }

            if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)  //←入力時
            {
                if (cursol2 == 0)
                {
                    vol_BGM -= 0.5f;
                    if (vol_BGM < -30) { vol_BGM = -30; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                else if(cursol2 == 1)
                {
                    //_Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE -= 0.5f;
                    if (vol_SE < -30) { vol_SE = -30; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                isHorizontal = true;
            }
            else if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)    //→入力時
            {
                if (cursol2 == 0)
                {
                    vol_BGM += 0.5f;
                    source_BGM = false;
                    if (vol_BGM > 20) { vol_BGM = 20; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                else if(cursol2 == 1)
                {
                    //_Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE += 0.5f;
                    source_SE = false;
                    if (vol_SE > 20) { vol_SE = 20; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                isHorizontal = true;
            }

            if (Input.GetButtonDown("LB"))
            {
                if (cursol2 == 0)
                {
                    vol_BGM -= 0.5f * 5;
                    if (vol_BGM < -30) { vol_BGM = -30; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                else if (cursol2 == 1)
                {
                    //_Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE -= 0.5f * 5;
                    if (vol_SE < -30) { vol_SE = -30; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
            }
            if (Input.GetButtonDown("RB"))
            {
                if (cursol2 == 0)
                {
                    vol_BGM += 0.5f * 5;
                    source_BGM = false;
                    if (vol_BGM > 20) { vol_BGM = 20; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
                else if (cursol2 == 1)
                {
                    //_Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE += 0.5f * 5;
                    source_SE = false;
                    if (vol_SE > 20) { vol_SE = 20; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                    gameSECS.audioSource.PlayOneShot(gameSECS.cursor);
                }
            }
        }
        if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;
        if (0 == Input.GetAxis("ClossHorizontal") && !isBlinking) isHorizontal = false;

        if (Input.GetButtonDown("A"))
        {
            if (cursol == 0)
            {
                if (!isSound)
                {
                    isSound = true;
                    gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
                }
                else if (cursol2 == 2 && isSound)
                {
                    using (StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/Volume"))
                    //("../Resources/Volume"))
                    {
                        sw.WriteLine(vol_BGM);
                        sw.WriteLine(vol_SE);
                    }

                    cursol2 = 0;
                    isSound = false;
                    gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
                }
            }
            else if (cursol == 1&& !isBlinking)
            {
                if (!isBack)
                {
                    isBack = true;
                    isBlinking = true;
                    Invoke("ToTitle",1f);

                    gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
                }
            }
        }
        else if (Input.GetButtonDown("B")&&!isBlinking)
        {
            if (cursol == 0&&isSound)
            {
               cursol2 = 0;
                isSound = false;
                gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
            }

            else if(cursol == 0&&!isSound)
            {
                cursol = 1;
                ButtonSize();
                gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
            }

            else if(cursol == 1)
            {
                isBack = true;
                isBlinking = true;
                Invoke("ToTitle", 1f);
                gameSECS.audioSource.PlayOneShot(gameSECS.crickSE);
            }
        }
        if (isBlinking) Blinking();
    }

    void ButtonSize()
    {
        if (cursol == 0)
        {
            obj_Sound.GetComponent<RectTransform>().localScale = new Vector3(1.728f, 1.728f, 0);
            obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);
            obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);


            obj_Sound.GetComponent<Image>().color = new Color(1f, 1f, 0, 1f);
            obj_Back.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        /*obj_Cursol.GetComponent<RectTransform>().transform.localPosition
            = obj_Sound.GetComponent<RectTransform>().transform.localPosition;

        obj_Cursol.GetComponent<RectTransform>().localScale
            = obj_Sound.GetComponent<RectTransform>().localScale;
    }/*
    else if (cursol == 1)
    {
        obj_Sound.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);
        obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.728f, 1.728f, 0);
        obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);

        obj_Cursol.GetComponent<RectTransform>().transform.localPosition
           = obj_Graphic.GetComponent<RectTransform>().transform.localPosition;
    } */
        else if (cursol == 1)
        {
            obj_Sound.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);
            obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);
            obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.728f, 1.728f, 0);

            /*obj_Cursol.GetComponent<RectTransform>().transform.localPosition
               = obj_Back.GetComponent<RectTransform>().transform.localPosition;

            obj_Cursol.GetComponent<RectTransform>().localScale
               = obj_Back.GetComponent<RectTransform>().localScale;*/

            obj_Sound.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            obj_Back.GetComponent<Image>().color = new Color(1f, 1f, 0, 1f);
        }
        }

        void ButtonSize2()
        {
            if (cursol2 == 0)
            {
                /*obj_Cursol2.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);

                obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
                    = new Vector3(5.0f, 100.0f, 0);

                obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(310f,40f);*/

                //font
                {
                    text_BGM.color = new Color32(255, 255, 0, 255);
                    text_BGM_num.color = new Color32(255, 255, 0, 255);

                    text_SE.color = new Color32(170, 255, 255, 127);
                    text_SE_num.color = new Color32(170, 255, 255, 127);

                    text_BGM.fontSize = 50;
                    text_BGM_num.fontSize = 50;

                    text_BGM.transform.localPosition = new Vector3(-200, 55, 0);
                    text_BGM_num.transform.localPosition = new Vector3(200, 55, 0);

                    text_SE.fontSize = 40;
                    text_SE_num.fontSize = 40;

                    text_SE.transform.localPosition = new Vector3(-200, -50, 0);
                    text_SE_num.transform.localPosition = new Vector3(200, -50, 0);

                    obj_Gauge_BGM.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
                    obj_Gauge_SE.GetComponent<Image>().color = new Color32(170, 255, 255, 127);
                }

                vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.2f, 1.2f, 0);
            vol_obj_Back.GetComponent<Image>().color = new Color32(170, 255, 255, 255);
            }
            else if (cursol2 == 1)
            {/*
            obj_Cursol2.GetComponent<RectTransform>().localScale = new Vector3(1.44f, 1.44f, 0);
            obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
               = new Vector3(5.0f, 0.0f, 0);
            obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(310f, 40f);*/

                //font
                {
                    text_BGM.color = new Color32(170, 255, 255, 127);
                    text_BGM_num.color = new Color32(170, 255, 255, 127);

                    text_SE.color = new Color32(255, 255, 0,  255);
                    text_SE_num.color = new Color32(255, 255, 0, 255);

                    text_BGM.fontSize = 40;
                    text_BGM_num.fontSize = 40;

                    text_BGM.transform.localPosition = new Vector3(-200, 50, 0);
                    text_BGM_num.transform.localPosition = new Vector3(200, 50, 0);

                    text_SE.fontSize = 50;
                    text_SE_num.fontSize = 50;

                    text_SE.transform.localPosition = new Vector3(-200, -45, 0);
                    text_SE_num.transform.localPosition = new Vector3(200, -45, 0);
                }

                vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.2f, 1.2f, 0);
                vol_obj_Back.GetComponent<Image>().color = new Color32(170, 255, 255, 255);

                obj_Gauge_BGM.GetComponent<Image>().color = new Color32(170, 255, 255, 127);
                obj_Gauge_SE.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
            }
            else if (cursol2 == 2)
            {/*
            obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(160f,40f);
            obj_Cursol2.GetComponent<RectTransform>().localScale= new Vector3(1.728f, 1.728f, 0);
            obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
               = vol_obj_Back.GetComponent<RectTransform>().transform.localPosition;*/
                //font
                {
                    text_BGM.color = new Color32(170, 255, 255, 127);
                    text_SE.color = new Color32(170, 255, 255, 127);

                    text_BGM_num.color = new Color32(170, 255, 255, 127);
                    text_SE_num.color = new Color32(170, 255, 255, 127);

                    text_BGM.fontSize = 40;
                    text_BGM_num.fontSize = 40;

                    text_SE.fontSize = 40;
                    text_SE_num.fontSize = 40;

                    text_BGM.transform.localPosition = new Vector3(-200, 50, 0);
                    text_BGM_num.transform.localPosition = new Vector3(200, 50, 0);

                    text_SE.transform.localPosition = new Vector3(-200, -50, 0);
                    text_SE_num.transform.localPosition = new Vector3(200, -50, 0);
                }

                obj_Gauge_BGM.GetComponent<Image>().color = new Color32(170, 255, 255, 127);
                obj_Gauge_SE.GetComponent<Image>().color = new Color32(170, 255, 255, 127);

                vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.44f, 1.44f, 0);
                vol_obj_Back.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
            }
        }

        void Slider()
        {
            obj_Gauge_SE.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Floor(((vol_SE + 30) / 50) * 100) * 3.6f, 25);
            obj_Gauge_BGM.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Floor(((vol_BGM + 30) / 50) * 100) * 3.6f, 25);
        }

        void ToTitle()
        {
            SceneManager.LoadScene("Title");
        }

        void Blinking()
        {
            blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
                                                                            //GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
        switch (cursol)
        {
            case 0:
                obj_Sound.GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
                break;
            case 1:
                obj_Back.GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
                break;
            case 2:
                vol_obj_Back.GetComponent<Image>().color = new Color(255, 255, 0, Mathf.Abs(blinking));  //絶対値でsin波を透明度に 点滅
                break;
        }
    }
}


