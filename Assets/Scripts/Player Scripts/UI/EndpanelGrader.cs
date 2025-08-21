using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndpanelGrader : MonoBehaviour
{
    [Serializable]
    public class Rating
    {
        public string grade;
        public string comment;
        public Color col;
        public float score;
    }


    public TMP_Text kills;
    public TMP_Text time;
    public TMP_Text deaths;
    public TMP_Text killsP;
    public TMP_Text timeP;
    public TMP_Text deathsP;
    public TMP_Text killsM;
    public TMP_Text timeM;
    public TMP_Text deathsM;
    public TMP_Text total;
    public TMP_Text grade;
    public TMP_Text comment;

    public TMP_Text timer;
    public List<Rating> ratings;

    float reqTime = 300;
    bool ended = false;

    DateTime start;
    public void Start ( )
    {
        start = DateTime.Now;
        reqTime = FindFirstObjectByType<PrerunSettings>().time;
    }

    public void Update ( )
    {
        if ( FinishLine.end )
        {
            if ( !ended )
            {
                Endgame( );
                ended = true;
            }
        }
        else
        {
            float secs = Mathf.Round((float)(DateTime.Now - start).TotalSeconds * 1000) / 1000;
            timer.text = $"{Mathf.Round( (secs % 60) * 1000 ) / 1000}s {( int ) secs / 60}m";
        }
    }

    public void Endgame( )
    {
        float seconds = (float)(DateTime.Now - start).TotalSeconds;
        int neededkills = EnemyCounter.enemies.maximum;
        int killsC = EnemyCounter.enemies.maximum - EnemyCounter.enemies.counted;
        int deathsC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>( ).deaths;
        float percentageK = 0;

        percentageK += Mathf.Round( EnemyCounter.enemies.invPercentage * 150 );
        kills.text = $"{killsC} kills";
        killsP.text = $"{percentageK}%";
        killsM.text = $"({neededkills})";

        float percentageD = 100 - (20 * deathsC);
        percentageD = Mathf.Clamp( percentageD, -200, 100 );
        deaths.text = $"{deathsC} deaths";
        deathsP.text = $"{percentageD}%";
        deathsM.text = $"(0)";

        float percentageT = Mathf.Round( reqTime / seconds * 100 );
        percentageT = Mathf.Clamp( percentageT, 50, 200 );
        if ( percentageT == 50f )
        {
            percentageT = 0;
        }
        time.text = $"{seconds}s";
        timeP.text = $"{percentageT}%";
        timeM.text = $"({reqTime}s)";


        float sum = percentageK + percentageD + percentageT;
        sum /= 3;
        sum = Mathf.Round( sum );
        total.text = $"{sum}%";

        foreach (var item in ratings)
        {
            if ( sum <= item.score )
            {
                grade.text = item.grade;
                grade.color = item.col;
                comment.text = item.comment;
            }
        }
    }
}
