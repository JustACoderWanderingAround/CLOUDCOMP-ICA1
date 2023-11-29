using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerDataScriptableObject metaDataSO;
    [SerializeField] private TMP_Text statisticsText;

    // Start is called before the first frame update
    void Start()
    {
        int minutes = metaDataSO.stats.timePlayed / 60;
        int seconds = metaDataSO.stats.timePlayed % 60;
        string statString = "Player Statistics\n";
        statString += "Level: " + metaDataSO.stats.level;
        statString += "XP: " + metaDataSO.stats.xp;
        statString += ("Meteor Kill Count: " + metaDataSO.stats.meteorKillCount + "\n");
        statString += ("Time Played: " + minutes.ToString() +" minutes " + seconds.ToString() + " seconds" + "\n");
        statString += ("Number of times played: " + metaDataSO.stats.timesPlayed + "\n");
        
        statisticsText.text = statString;
    }
}
 