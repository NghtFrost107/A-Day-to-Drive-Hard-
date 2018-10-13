using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLite4Unity3d;

public class Score  {

    [PrimaryKey, AutoIncrement]
    public int scoreID { get; set; }
    public string time { get; set; }
    public string date { get; set; }
    public int score { get; set; }

    public override string ToString()
    {
        return string.Format("{0,3}: {0,10}  {2} {3}",scoreID,score,time,date);
    }

}
