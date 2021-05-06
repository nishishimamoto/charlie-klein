using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer mixer;

    float vol_SE,vol_BGM;

    
   
    
    
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

    }


}