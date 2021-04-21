using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer mixer;

    float vol_SE,vol_BGM;

    bool axis_X, axis_Y;
    public bool isBGM;

    // Start is called before the first frame update
    void Start()
    {
        mixer.GetFloat("SE", out vol_SE);
        mixer.GetFloat("SE", out vol_BGM);
    }

    // Update is called once per frame
    void Update()
    {
       /* mixer.SetFloat("SE", vol_SE);
        mixer.SetFloat("SE", vol_BGM);
       */
        


        //十字キーのパネル選択
        if ((0 > Input.GetAxis("ClossVertical") && !axis_Y)
         || (0 < Input.GetAxis("ClossVertical") && !axis_Y))   //↓入力時
        {

            if (isBGM)
            {
                Debug.Log("isBGM:" + isBGM);
                isBGM = false;
            }
            else 
            {
                Debug.Log("isBGM:" + isBGM);
                isBGM = true;

            }
           // axis_Y = true;
            
        }

        if (0 > Input.GetAxis("ClossHorizontal") && !axis_X)  //←入力時
        {
            if (isBGM)
            {
                vol_BGM -= 0.8f;
                if (vol_BGM < -80) { vol_BGM = -80; }
                Debug.Log("vol_BGM = " + vol_BGM);
                mixer.SetFloat("BGM", vol_BGM);

            }
            else
            {
                vol_SE -= 0.8f;
                if (vol_SE < -80) { vol_SE = -80; }
                Debug.Log("vol_SE = " + vol_SE);
                mixer.SetFloat("SE", vol_SE);

            }
            axis_X = true;
        }
       
        if (0 < Input.GetAxis("ClossHorizontal") && !axis_X)    //→入力時
        {
            if (isBGM)
            {
                vol_BGM += 0.8f;
                if (++vol_BGM > 0) { vol_BGM = 0; }
                Debug.Log("vol_BGM = " + vol_BGM);
                mixer.SetFloat("BGM", vol_BGM);
            }
            else
            {
                vol_SE += 0.8f;
                if (++vol_SE > 0) { vol_SE = 0; }
                Debug.Log("vol_SE = " + vol_SE);
                mixer.SetFloat("SE", vol_SE);
            }
        }

            axis_X = true;

        if (0 == Input.GetAxis("ClossHorizontal")) axis_X = false;
        //if (0 == Input.GetAxis("ClossVertical")) axis_Y = false;

        if(Input.GetButtonDown("LB"))
        {
            if(isBGM)
            {
                vol_BGM -= 0.8f*5;
                if (vol_BGM < -80) { vol_BGM = -80; }
                Debug.Log("vol_BGM = " + vol_BGM);
                mixer.SetFloat("BGM", vol_BGM);

            }
            else
            {
                vol_SE -= 0.8f*5;
                if (vol_SE < -80) { vol_SE = -80; }
                Debug.Log("vol_SE = " + vol_SE);
                mixer.SetFloat("SE", vol_SE);

            }
        }
        if (Input.GetButtonDown("RB"))
        {
            if (isBGM)
            {
                vol_BGM += 0.8f * 5;
                if (vol_BGM > 0) { vol_BGM = 0; }
                Debug.Log("vol_BGM = " + vol_BGM);
                mixer.SetFloat("BGM", vol_BGM);

            }
            else
            {
                vol_SE += 0.8f * 5;
                if (vol_SE > 0) { vol_SE = 0; }
                Debug.Log("vol_SE = " + vol_SE);
                mixer.SetFloat("SE", vol_SE);

            }
        }
    }


}