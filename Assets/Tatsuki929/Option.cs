using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Option : MonoBehaviour
{
    public AudioMixer mixer;

    float vol_SE, vol_BGM;

    short cursol = 0, cursol_old = 0, cursol2 = 0, cursol_old2 = 0;

    public bool isVertical, isHorizontal, isSound, isGraph, isBack, isBlinking;

    [SerializeField] GameObject obj_Sound;
    [SerializeField] GameObject obj_Graphic;
    [SerializeField] GameObject obj_Back;

    [SerializeField] GameObject obj_Cursol;
    [SerializeField] GameObject obj_Cursol2;

    [SerializeField] GameObject vol_Setting;
    [SerializeField] GameObject vol_obj_Back;

    [SerializeField] GameObject vol_obj_SE;
    [SerializeField] GameObject vol_obj_BGM;

    [SerializeField] Text text_BGM;
    [SerializeField] Text text_SE;

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
        if (!isBack && !isGraph && !isSound)
        {
            obj_Sound.SetActive(true);
            obj_Graphic.SetActive(true);
            obj_Back.SetActive(true);
            obj_Cursol.SetActive(true);
            vol_Setting.SetActive(false);
            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                 cursol_old = cursol;
                //if (cursol >= 8) cursol -= 8;
                //else cursol += 2;
                if (cursol >= 2) cursol -= 2;
                else cursol += 1;
                isVertical = true;
                ButtonSize();
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                cursol_old = cursol;
                //if (cursol <= 1) cursol += 8;
                //else cursol -= 2;
                if (cursol <= 0) cursol += 2;
                else cursol -= 1;
                isVertical = true;
                ButtonSize();
            }
        }
        else if(isSound)
        {
            obj_Sound.SetActive(false);
            obj_Graphic.SetActive(false);
            obj_Back.SetActive(false);
            obj_Cursol.SetActive(false);
            vol_Setting.SetActive(true);


            ButtonSize2();
            Slider();

            text_BGM.text = Mathf.Floor(((vol_BGM+80)/80)*100).ToString();
            text_SE.text = Mathf.Floor(((vol_SE + 80) / 80)*100).ToString();

            if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
            {
                cursol_old2 = cursol2;
                if (cursol2 >= 2) cursol2 -= 2;
                else cursol2 += 1;
                isVertical = true;
                //ButtonSize2();
            }
            else if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
            {
                cursol_old2 = cursol2;

                if (cursol2 <= 0) cursol2 += 2;
                else cursol2 -= 1;
                isVertical = true;
                //ButtonSize2();
            }

            if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)  //←入力時
            {
                if (cursol2 == 0)
                {
                    vol_BGM -= 0.8f;
                    if (vol_BGM < -80) { vol_BGM = -80; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);

                }
                else if(cursol2 == 1)
                {
                    _Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE -= 0.8f;
                    if (vol_SE < -80) { vol_SE = -80; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);

                }
                isHorizontal = true;
            }
            else if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)    //→入力時
            {
                if (cursol2 == 0)
                {
                    vol_BGM += 0.8f;
                    if (vol_BGM > 0) { vol_BGM = 0; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);
                }
                else if(cursol2 == 1)
                {
                    _Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE += 0.8f;
                    if (vol_SE > 0) { vol_SE = 0; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                  
                }
                isHorizontal = true;
            }

            if (Input.GetButtonDown("LB"))
            {
                if (cursol2 == 0)
                {
                    vol_BGM -= 0.8f * 5;
                    if (vol_BGM < -80) { vol_BGM = -80; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);

                }
                else if (cursol2 == 1)
                {
                    _Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE -= 0.8f * 5;
                    if (vol_SE < -80) { vol_SE = -80; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);

                }
            }
            if (Input.GetButtonDown("RB"))
            {
                if (cursol2 == 0)
                {
                    vol_BGM += 0.8f * 5;
                    if (vol_BGM > 0) { vol_BGM = 0; }
                    Debug.Log("vol_BGM = " + vol_BGM);
                    mixer.SetFloat("BGM", vol_BGM);

                }
                else if (cursol2 == 1)
                {
                    _Cursol.SE.PlayOneShot(_Cursol.move);
                    vol_SE += 0.8f * 5;
                    if (vol_SE > 0) { vol_SE = 0; }
                    Debug.Log("vol_SE = " + vol_SE);
                    mixer.SetFloat("SE", vol_SE);
                }
            }
        }
        if (0 == Input.GetAxis("ClossVertical") && !isBlinking) isVertical = false;
        if (0 == Input.GetAxis("ClossHorizontal") && !isBlinking) isHorizontal = false;

        if (Input.GetButtonDown("A"))
        {
            Debug.Log("マジで押してるん？");
            if (cursol == 0)
            {
                if (!isSound)
                {
                    isSound = true;
                }
                else if (cursol2 == 2 && isSound)
                {
                    cursol2 = 0;
                    isSound = false;
                }
            }
            else if (cursol == 1)
            {
                if (!isGraph) 
                {
                    isGraph = true;
                }
                else
                {
                    isGraph = false;
                }
            }
            else if (cursol == 2)
            {
                if (!isBack)
                {
                    isBack = true;

                    SceneManager.LoadScene("Title");
                }
            }
        }
        else if (Input.GetButtonDown("B"))
            {
                if (cursol == 0)
                {
                    cursol2 = 0;
                    isSound = false;
                }
            }
    }

    void ButtonSize()
    {
        if (cursol == 0)
        {
            obj_Sound.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
            obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
            obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);

            obj_Cursol.GetComponent<RectTransform>().transform.localPosition
                = obj_Sound.GetComponent<RectTransform>().transform.localPosition;
        }
        else if(cursol == 1)
        {
            obj_Sound.GetComponent<RectTransform>().localScale =  new Vector3(1.0f, 1.0f, 0);
            obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
            obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);

            obj_Cursol.GetComponent<RectTransform>().transform.localPosition
               = obj_Graphic.GetComponent<RectTransform>().transform.localPosition;
        }
        else if (cursol == 2)
        {
            obj_Sound.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
            obj_Graphic.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 0);
            obj_Back.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);

            obj_Cursol.GetComponent<RectTransform>().transform.localPosition
               = obj_Back.GetComponent<RectTransform>().transform.localPosition;
        }
    }

    void ButtonSize2()
    {
        if (cursol2 == 0)
        {
            obj_Cursol2.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);

            obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
                = new Vector3(5.0f, 100.0f, 0);
            
            obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(310f,40f);
            
            vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 0);
        }
        else if (cursol2 == 1)
        {
            obj_Cursol2.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 0);
            obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
               = new Vector3(5.0f, 0.0f, 0);
            obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(310f, 40f);

            vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 0);
        }
        else if (cursol2 == 2)
        {
            obj_Cursol2.GetComponent<RectTransform>().sizeDelta = new Vector2(160f,40f);
            obj_Cursol2.GetComponent<RectTransform>().localScale= new Vector3(1.44f, 1.44f, 0);
            obj_Cursol2.GetComponent<RectTransform>().transform.localPosition
               = vol_obj_Back.GetComponent<RectTransform>().transform.localPosition;

            vol_obj_Back.GetComponent<RectTransform>().transform.localScale = new Vector3(1.2f, 1.2f, 0);
        }
    }

    void Slider()
    {
        obj_Gauge_SE.GetComponent<RectTransform>().transform.localPosition = new Vector3(-180f+ Mathf.Floor(((vol_BGM + 80) / 80) * 100)*3.6f, 100,0);
        obj_Gauge_BGM.GetComponent<RectTransform>().transform.localPosition = new Vector3(-180f+ Mathf.Floor(((vol_SE + 80) / 80) * 100)*3.6f, 0,0);
    }
}