using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer mixer;

    float vol_SE,vol_BGM;


    [SerializeField] AudioSource BGM, SE;

    public Option option;
    // Start is called before the first frame update
    void Start()
    {
        
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
                SE.mute = true;
            }

            if (vol_BGM <= -30)
            {
                BGM.mute = true;
            }
        }
    }


}