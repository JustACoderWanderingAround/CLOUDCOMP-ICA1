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
        string statString = "Player Statistics\n";
        statString += ("Meteor Kill Count: " + metaDataSO.Stats.meteorKillCount + "\n");
        statString += ("Meteor Escape Count: " + metaDataSO.Stats.meteorKillCount + "\n");
        statString += ("Time Played: " + metaDataSO.Stats.timePlayed + "\n");
        statString += ("Number of times play: " + metaDataSO.Stats.timesPlayed + "\n");
        statisticsText.text = statString;
    }
}
