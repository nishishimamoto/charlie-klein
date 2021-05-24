using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer mixer;

    float vol_SE,vol_BGM;


    [SerializeField] AudioSource BGM, SE, pauseSE;

    public Option option;

    GameObject game;
    GameObject pause;
    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.Find("Canvas/Pause");
        game = GameObject.Find("GameSE");
        if (game) SE = game.GetComponent<AudioSource>();
        if(pause) pauseSE = pause.GetComponent<AudioSource>();

        using (StreamReader sr = new StreamReader(Application.dataPath+ "/Resources/Volume"))
        {
            vol_BGM = float.Parse(sr.ReadLine());
            vol_SE = float.Parse(sr.ReadLine());
        }
        mixer.SetFloat("SE", vol_SE);
        mixer.SetFloat("BGM", vol_BGM);
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (option != null)
        {
            vol_BGM = option.vol_BGM;
            vol_SE = option.vol_SE;
        }
        
        {
            if (vol_SE <= -30)
            {
                if (SE != null)
                    SE.mute = true;
                if (pauseSE != null)
                    pauseSE.mute = true;
            }
            else if(vol_SE>=-29)
            {
                if (SE != null)
                    SE.mute = false;
                if (pauseSE != null)
                    pauseSE.mute = false;
            }

            if (vol_BGM <= -30)
            {
                if(BGM != null)
                BGM.mute = true;
            }
            else if (vol_BGM >= -29)
            {
                if (BGM != null)
                    BGM.mute = false;
            }
        }
    }


}