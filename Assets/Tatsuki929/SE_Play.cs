using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SE_Play : MonoBehaviour
{
    public AudioSource SE;

    bool nanika;

    [SerializeField] Pause pause;

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

        nanika = pause.isPause;

        if ((Input.GetButtonDown("LB")|| Input.GetButtonDown("RB")) && !nanika)
        {
            SE.PlayOneShot(SE.clip);
        }
    }
}
