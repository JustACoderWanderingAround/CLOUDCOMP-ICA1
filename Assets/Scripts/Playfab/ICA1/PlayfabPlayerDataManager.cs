using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabPlayerDataManager : MonoBehaviour
{
    public void SetUserData(string dataKey, string dataValue)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {dataKey, dataValue}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error settling user " + dataKey);
            Debug.Log(error.GenerateErrorReport());
        }
        );
    }
    public string GetUserData()
    {
        string returnString = " ";
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {

        },
        result =>
        {
            Debug.Log("Got user data: ");
            if (result.Data == null || !result.Data.ContainsKey("XP")) Debug.Log("No XP");
            else
            {
                Debug.Log("XP: " + result.Data["XP"].Value);
                returnString =  result.Data["XP"].Value;
                //XPDisplay.text = "XP:" + result.Data["XP"].Value;
            }
        },
        (error) =>
        {
            Debug.Log("Got error retriving user data");
            Debug.Log(error.GenerateErrorReport());
        });
        return returnString;
    }
    
}
