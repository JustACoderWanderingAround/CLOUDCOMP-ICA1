using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

public class FriendRow : MonoBehaviour
{
    [SerializeField] TMP_Text nameText, tagText;
    [SerializeField] GameObject buttons;
    private string friendName, friendTags, friendID;
    public void InitRow(string name, string tags, string friendID)
    {
        nameText.text = name;
        friendName = name;
        tagText.text = tags;
        friendTags = tags;
        if (tags.Contains("requester"))
        {
            buttons.SetActive(true);
        }
        this.friendID = friendID;
    }
    public void PressAddFriendButton()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new { FriendPlayFabId = friendName },
            GeneratePlayStreamEvent = true,
        }, result =>
        {
            Debug.Log("Accepted successfully!");
        }, null);
    }
    public void RemoveFriend(string pfid)
    {
        var req = new RemoveFriendRequest
        {
            FriendPlayFabId = pfid
        };
        PlayFabClientAPI.RemoveFriend(req
        , result => {
            Debug.Log("Unfriended successfully!");
        }, OnError);
    }
    public void PressRemoveFriendButton()
    {
        RemoveFriend(friendID);
    }
    void OnError(PlayFabError r)
    {
        Debug.Log("Error: friendRow" + r.GenerateErrorReport());
    }
}
