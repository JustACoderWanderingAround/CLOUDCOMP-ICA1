using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    public int level;
    public int xp;
    public int meteorKillCount;
    public int timePlayed;
    public int timesPlayed;
    public int highScore;
}

[CreateAssetMenu(fileName = "playerData")]
public class PlayerDataScriptableObject : ScriptableObject
{
   
    public PlayerStats stats;
   


    public string SaveToString()
    {
        return JsonUtility.ToJson(stats);
    }

    public void ResetData()
    {
        stats.level = 0;
        stats.xp = 0;
        stats.meteorKillCount = 0;
        stats.timePlayed = 0;
        stats.timesPlayed = 0;
        stats.highScore = 0;
    }
}
