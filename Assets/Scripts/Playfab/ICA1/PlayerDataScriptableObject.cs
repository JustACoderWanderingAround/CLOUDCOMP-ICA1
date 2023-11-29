using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "playerData")]
public class PlayerDataScriptableObject : ScriptableObject
{
    [Serializable]
    public struct PlayerStats
    {
        public string meteorKillCount;
        public string meteorEscapeCount;
        public string timePlayed;
        public string timesPlayed;
    }
    [SerializeField]
    private PlayerStats stats;
    public PlayerStats Stats { get => stats; set => stats = value; }


    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void ResetData()
    {
        stats.meteorKillCount = "0";
        stats.meteorEscapeCount = "0";
        stats.timePlayed = "0";
        stats.timesPlayed = "0";
    }
}
