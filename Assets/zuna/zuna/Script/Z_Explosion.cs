using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_Explosion : MonoBehaviour
{
    // Inspector
    public ParticleSystem[] particle = new ParticleSystem[42];
    [SerializeField] ParticleSystem explosopn;
    public AudioSource audio;
    public AudioClip clip;
    
    

    private void Start()
    {audio.playOnAwake = false;//起動時の再生を無効に
        clip = Resources.Load<AudioClip>("expl3");//Resourceから読み込み
        //audio.PlayOneShot(clip);//再生

        for (int i = 0; i < 42; i++)
        {
            particle[i] = Instantiate(explosopn, new Vector3(-5 + (2 * (i % 7 - 1)), 5 - (2 * (i / 7)), 0.0f), Quaternion.identity);

        }
    }
}
