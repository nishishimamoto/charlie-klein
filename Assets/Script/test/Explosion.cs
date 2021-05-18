using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Inspector
    public ParticleSystem[] particle = new ParticleSystem[30];
    public ParticleSystem[] bom;
    [SerializeField] ParticleSystem explosopn;
    public AudioSource audio;
    public AudioClip clip;
    
    

    private void Start()
    {audio.playOnAwake = false;//起動時の再生を無効に
        clip = Resources.Load<AudioClip>("expl3");//Resourceから読み込み
        //audio.PlayOneShot(clip);//再生

        for (int i = 0; i < 30; i++)
        {
            particle[i] = Instantiate(explosopn, new Vector3(-6 + (2 * (i % 6)), 3.5f - (2 * (i / 6)), -2.0f), Quaternion.identity);
            
        }
    }

    public void BomInit(int side)
    {
        bom = new ParticleSystem[side];

        for (int i = 0; i < side; i++)
        {
            bom[i] = Instantiate(explosopn, new Vector3(-7 + (2 * (i % 7)), 5 - (2 * (i / 7)), -1.0f), Quaternion.identity);
        }
    }
}
