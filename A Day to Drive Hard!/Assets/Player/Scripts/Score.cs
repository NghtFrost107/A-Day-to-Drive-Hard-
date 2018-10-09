using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score  {

    public string time;
    public string date;
    public int score;

    public Score(int _score)
    {
        time = System.DateTime.Now.ToShortTimeString();
        date = System.DateTime.Now.ToShortDateString();
        score = _score;
    }
}
