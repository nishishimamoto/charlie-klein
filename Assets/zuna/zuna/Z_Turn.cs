﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Z_Turn: MonoBehaviour
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
        scoreText.text = "" + (test2.TurnMax - test2.turn);
        
    }
}