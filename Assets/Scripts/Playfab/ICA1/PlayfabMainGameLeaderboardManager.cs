using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class PlayfabMainGameLeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text leaderboardText;

    public void ResetLeaderboardText()
    {
        leaderboardText.text = "";
    }
    
    public void GetTopLeaderboard()
    {
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "Highscore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(lbreq, OnSuccessLeaderboardGet, OnError);
    }

    public void GetLocalLeaderboard()
    {
        var lbreq = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "Highscore",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(lbreq, OnSuccessPlayerLeaderboardGet, OnError);
    }
    
    void OnSuccessLeaderboardGet(GetLeaderboardResult r)
    {
        string LeaderboardStr = "Leaderboard: \n";
        Debug.Log("Scoreboard get success!");
        if (r.Leaderboard.Count > 0)
        {
            LeaderboardStr += "--- LEADERBOARD---\n";
            foreach (var item in r.Leaderboard)
            {

                string onerow = item.Position + 1 + " - " + item.PlayFabId + " - " + item.DisplayName + " - " + item.StatValue + "\n";
                LeaderboardStr += onerow;
            }
            leaderboardText.text = LeaderboardStr;
        } 
        else
        {
            leaderboardText.text = "--- LEADERBOARD---\nLeaderboard is empty;";
        }
        
    }
    void OnSuccessPlayerLeaderboardGet(GetLeaderboardAroundPlayerResult r)
    {
        string LeaderboardStr = "Leaderboard: \n";
        Debug.Log("Scoreboard get success!");
        if (r.Leaderboard.Count > 0)
        {
            LeaderboardStr += "--- LEADERBOARD---\n";
            foreach (var item in r.Leaderboard)
            {

                string onerow = item.Position + 1 + " - " + item.PlayFabId + " - " + item.DisplayName + " - " + item.StatValue + "\n";
                LeaderboardStr += onerow;
            }
            leaderboardText.text = LeaderboardStr;
        } 
        else
        {
            leaderboardText.text = "--- LEADERBOARD---\nLeaderboard is empty;";
        }
        
    }
    //void RetrieveUsernameByID(out string usernameString)
    //{
    //    usernameString = "";
    //    GetAccountInfoRequest request = new GetAccountInfoRequest();
    //    PlayFabClientAPI.GetAccountInfo(request, , OnUsernameRetrieveFail);
    //}
    void OnUsernameRetrieveFail(PlayFabError r)
    {
        Debug.Log("Error: MGLBManager username retrieve fail" + r.GenerateErrorReport());

    }
    void OnError(PlayFabError r)
    {
        Debug.Log("Error: MGLBManager" + r.GenerateErrorReport());
    }
}
