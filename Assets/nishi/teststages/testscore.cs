﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class testscore : MonoBehaviour
{
    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Isha_Singlshot.score > 0)scoreText.text = "" + Isha_Singlshot.score;
        else if (test3.score > 0) scoreText.text = "" + test3.score;
        else if (test2.score > 0) scoreText.text = "" + test2.score;
    }
}