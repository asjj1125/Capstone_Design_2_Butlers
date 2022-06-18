using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreText : MonoBehaviour
{
    float score;
    void Start()
    {
        if(!PlayerPrefs.HasKey("playerScore"))
        {
            return;
        }
        score = PlayerPrefs.GetFloat("playerScore", 0);
        TimeSpan time = TimeSpan.FromSeconds(score);
        this.GetComponent<Text>().text = time.ToString("mm") + "�� " + time.ToString("ss") + "��";
        this.GetComponent<Text>().fontSize = 80;
    }

    void Update()
    {
        
    }
}
