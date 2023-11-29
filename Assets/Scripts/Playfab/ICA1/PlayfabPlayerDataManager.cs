using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabPlayerDataManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    Dictionary<string, UserDataRecord> playerDataDict;
    IEnumerator WaitOneSecond()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
    }
    IEnumerator WaitForNSeconds(int n)
    {
        yield return new WaitForSeconds(n);
    }
    private void OnEnable()
    {
        WaitOneSecond();
        if (levelText != null && xpText != null)
        {
            StartCoroutine(WaitForNSeconds(2));
            GetUserData();
            var temp = new UserDataRecord();
        }
    }
    public void SetUserData(string dataKey, string dataValue)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {dataKey, dataValue}
            }
        },
        result => Debug.Log("Successfully updated user data" + dataKey),
        error =>
        {
            Debug.Log("Got error settling user " + dataKey);
            Debug.Log(error.GenerateErrorReport());
        }
        );
    }
    public void InitializePlayer()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"XP", "0"},
                {"LV", "0"}
            }
        },
       result => Debug.Log("Successfully initialized user data"),
       error =>
       {
           Debug.Log("Got error initalizingPlayer" );
           Debug.Log(error.GenerateErrorReport());
       }
       );
    }
    public void GetUserData()
    {
        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {},
        OnDataGetSuccess,
        (error) =>
        {
            Debug.Log("Got error retriving user data");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    void UpdateTextBox(TMP_Text textBox, string value)
    {
        textBox.text = value;
    }
    void OnDataGetSuccess(GetUserDataResult result)
    {
        playerDataDict = new Dictionary<string, UserDataRecord>();
        Debug.Log("Got user data");
        if (result.Data == null) Debug.Log("No Data");
        else
        {
            //Debug.Log("XP: " + result.Data["XP"].Value);
            //returnString =  result.Data["XP"].Value;
            ////XPDisplay.text = "XP:" + result.Data["XP"].Value;
            foreach (var item in result.Data)
            {
                Debug.Log("Data added: " + item.Key + "-" + item.Value + "(" + item.Value.Value + ")");
                playerDataDict.Add(item.Key, item.Value);
            }

        }
        
    }
}
