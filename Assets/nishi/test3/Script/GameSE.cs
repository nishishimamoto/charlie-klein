using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSE : MonoBehaviour
{
    public AudioClip comboSE;
    public AudioClip lifeSE;
    public AudioClip timerSE;
    public AudioClip turnSE;
    public AudioClip gameOverSE;
    public AudioClip bomChargeSE;
    public AudioClip thinkingSE;
    public AudioClip massSE;
    public AudioClip bomSE1;
    public AudioClip bomSE2;
    public AudioClip explosion1SE;
    public AudioClip explosion2SE;
    public AudioClip cursor;
    public AudioClip crick;
    public AudioClip magic;
    public AudioClip pause;
    public AudioClip mobius;
    public AudioClip spinSE;
    public AudioClip cursorSE;
    public AudioClip crickSE;
    public AudioClip pauseSE;
    public AudioClip successSE;
    public AudioClip Z_startSE;

    public bool isLifeSE;
    public bool isLifeSECheck;
    public bool is5countSE;
    public bool isBomSE;
    public bool isOneMassSE;

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
