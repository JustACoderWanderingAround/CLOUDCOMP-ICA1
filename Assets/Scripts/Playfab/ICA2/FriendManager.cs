using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

// written by paul baquiran 220150y gd2204
public class FriendManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtFrdList, leaderboarddisplay;
    [SerializeField] TMP_InputField nameInputField;
    // Start is called before the first frame update
    private void Awake()
    {
        GetFriends();
    }
    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        txtFrdList.text = "";
        friendsCache.ForEach(f => {
            Debug.Log(f.FriendPlayFabId + "," + f.TitleDisplayName);
            txtFrdList.text += f.TitleDisplayName + "[" + f.FriendPlayFabId + "]\n";
            if (f.Profile != null) Debug.Log(f.FriendPlayFabId + "/" + f.Profile.DisplayName);
        });
    }
    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }

    List<FriendInfo> _friends = null;
    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            // ExternalPlatformFriends = false,
            // XboxToken = null
        }, result => {
            _friends = result.Friends;
        }, DisplayPlayFabError);
    }
    enum FriendIdType { PlayFabId, Username, Email, DisplayName };
    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }
    public void OnAddFriend()
    { //to add friend based on display name
        AddFriend(FriendIdType.DisplayName, nameInputField.text);
    }
    // unlike AddFriend, RemoveFriend only takes a PlayFab ID
    // you can get this from the FriendInfo object under FriendPlayFabId
    void RemoveFriend(FriendInfo friendInfo)
    { //to investigat
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }
    public void OnUnFriend()
    {
        RemoveFriend(nameInputField.text);
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
        }, DisplayPlayFabError);
    }
    public void RemoveFriendByUsername(string username)
    {

    }

    public void AddFriendByUsername()
    {
        AddFriend(FriendIdType.Username, nameInputField.text);
    }
    public void AddFriendByDisplayName()
    {
        AddFriend(FriendIdType.DisplayName, nameInputField.text);
    }
    public void AddFriendByEmail()
    {
        AddFriend(FriendIdType.Email, nameInputField.text);
    }
    public void AddFriendByPlayFabID()
    {
        AddFriend(FriendIdType.PlayFabId, nameInputField.text);
    }   
    public List<FriendInfo> GetFriendList()
    {
        return _friends;
    }
    //void GiveItemTo(string secondPlayerID, string myItemInstanceID)
    //{
    //    PlayFabClientAPI.OpenTrade(new OpenTradeRequest
    //    {
    //        AllowedPlayerIds = new List<string> { secondPlayerID }, 
    //        OfferedInventoryInstanceIds = new List<string> { myItemInstanceID },
    //    }, LogSuccess, DisplayError);
    //}
    public void SendFriendReqByID()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest { 
            FunctionName = "SendFriendRequest", 
            FunctionParameter = new { FriendPlayFabId = nameInputField.text }, 
            GeneratePlayStreamEvent = true,
        }, result => {
            Debug.Log("Added successfully!");
        }, null);
    }
}
