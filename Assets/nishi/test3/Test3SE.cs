using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3SE : MonoBehaviour
{
    public AudioClip comboSE;
    public AudioClip lifeSE;
    public AudioClip timerSE;
    public AudioClip turnSE;
    public AudioClip gameOverSE;
    public AudioClip bomChargeSE;
    public AudioClip thinkingSE;
    public AudioClip massSE;
    public AudioClip explosion1SE;
    public AudioClip explosion2SE;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
