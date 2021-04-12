using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SE_Cursol : MonoBehaviour
{
    public AudioSource SE;
    public bool axis_ver, axis_hor;//軸の動き、Trueで左右カーソルを動かない
    public AudioClip move;
    public AudioClip dicide;
    // Start is called before the first frame update
    void Start()
    {
       /* SE = this.GetComponent<AudioSource>();*/

        SE.volume = 1;

       // SE.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("T_Audiomixer");
    }

    // Update is called once per frame
    void Update()
    {
    if ((0 > Input.GetAxis("ClossVertical") && !axis_ver)
     || (0 < Input.GetAxis("ClossVertical") && !axis_ver)
     || (0 > Input.GetAxis("ClossHorizontal") && !axis_ver && !axis_hor)
     || (0 < Input.GetAxis("ClossHorizontal") && !axis_ver && !axis_hor))  //↑入力時//↓入力時
            {
                axis_ver = true;
            
                SE.PlayOneShot(move);
            }

        
        else if ((0 == Input.GetAxis("ClossVertical")) && (0 == Input.GetAxis("ClossHorizontal")) )axis_ver = false;

        if (Input.GetButtonDown("A"))
        {
            SE.PlayOneShot(dicide);
        }
    }
}
