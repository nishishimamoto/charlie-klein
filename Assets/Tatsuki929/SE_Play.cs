using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SE_Play : MonoBehaviour
{
    public AudioSource SE;

    bool isPause,isPlay;

    [SerializeField] Pause pause;
    public Option option;
    // Start is called before the first frame update
    void Start()
    {
        SE = this.GetComponent<AudioSource>();

        SE.volume = 1;

        SE.clip = Resources.Load<AudioClip>("Earth_Tremor");

       // SE.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("T_Audiomixer");
    }

    // Update is called once per frame
    void Update()
    {
        if (pause != null) {
            isPause = pause.isPause;
        }

        if (option != null)
        {
            isPlay = option.source_SE;
        }

        if (isPlay)
        {
            SE.mute = true;
        }
        else
        {
            SE.mute = false;
        }
        
        if ((Input.GetButtonDown("LB")|| Input.GetButtonDown("RB")) && !isPause && option==null)
        {
            SE.PlayOneShot(SE.clip);
        }

    }
}
