using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public AudioMixerGroup Mixer;

    public Option option;

    private const short songs = 10;
    bool isPlay;

    public bool Play=false;//再生しているか　falseで未再生

    // Start is called before the first frame update
    void Start()
    {
        //オーディオソースの取得
        audioSource = this.GetComponent<AudioSource>();

        //オーディオミキサーの取得
        Mixer = Resources.Load<AudioMixerGroup>("T_Audiomixer");

        //クリップを動的に取得
        audioClips[0] = Resources.Load<AudioClip>("Electronic_Circuit");
        //audioClips[1] = Resources.Load<AudioClip>("untitled");

        

        //オーディオミキサーをオーディオソースに
       audioSource.outputAudioMixerGroup = Mixer;

        /*audioSource.outputAudioMixerGroup.audioMixer.SetFloat("T_Audiomixer/Master", -25.0f);
        //audioSource.outputAudioMixerGroup.*/

        audioSource.clip = audioClips[0];

        audioSource.volume = 0.25f;

        audioSource.loop = true;//ループを有効化

        audioSource.Play(); Play = true;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (option != null)
        {
            isPlay = option.source_BGM;
            if (isPlay)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
        }
        
        

        if (Input.GetKeyDown(KeyCode.Space))//スペースを押し下げたとき
        {

            if (!Play)
            {   //BGMを再生
                audioSource.Play();
                Play = true;
            }
            else 
            {   //BGMを停止
                audioSource.Stop();
                Play = false;
            }
        }


    }
}
