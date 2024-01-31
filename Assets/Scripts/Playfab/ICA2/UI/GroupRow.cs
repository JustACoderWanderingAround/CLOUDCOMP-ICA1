using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.GroupsModels;
// written by paul baquiran 220150y gd2204
public class GroupRow : MonoBehaviour
{
    public GameObject tick;
    public GameObject cross;
    public UnityEngine.UI.Button viewButton;
    private EntityKey ek;
    [SerializeField] private TMP_Text mainText;
    public void SetText(string newText)
    {
        mainText.text = newText;
    }
    public void SetID(EntityKey ek, bool toggleTick, bool toggleCross, bool toggleView = false)
    {
        this.ek = ek;
        var request = new GetGroupRequest()
        {
            Group = ek
        };
        PlayFabGroupsAPI.GetGroup(
            request,
            result =>
            {
                tick.SetActive(toggleTick);
                cross.SetActive(toggleCross);
                viewButton.gameObject.SetActive(toggleView);
                SetText(result.GroupName);
            }, (e) =>
            {
                SetText(e.ErrorMessage);
            }
        );
        
    }
    public void SetUserID(EntityKey ek, string roleName)
    {
        this.ek = ek;
        PlayFab.ProfilesModels.EntityKey newEntityKey = new PlayFab.ProfilesModels.EntityKey();
        newEntityKey.Id = ek.Id;
        newEntityKey.Type = "title_player_account";
        var request = new PlayFab.ProfilesModels.GetEntityProfileRequest()
        {
            Entity = newEntityKey
        };
        PlayFabProfilesAPI.GetProfile(request,
            r =>
            {
                string newString = new string("");
                newString += "Name: " + r.Profile.DisplayName + "\n";
                newString += "ID: " + ek.Id + "\n";
                newString += "Role: " + roleName;
                SetText(newString);
            },
            e => { Debug.LogError(e); });
    }
    public void AcceptInvite()
    {
        var request = new AcceptGroupInvitationRequest()
        {
            Group = ek,
        };
        PlayFabGroupsAPI.AcceptGroupInvitation(
            request,
            result =>
            {
                Destroy(gameObject);
            }, (e) =>
            {
                SetText(e.ErrorMessage); 
                Debug.LogError(e.GenerateErrorReport());
            }
        );
    }
    public void RejectInvite()
    {
        var request = new RemoveGroupInvitationRequest()
        {
            Group = ek,
        };
        PlayFabGroupsAPI.RemoveGroupInvitation(
            request,
            result =>
            {
                Destroy(gameObject);
            }, (e) =>
            {
                SetText(e.ErrorMessage);
            }
        );
    }
    public void RemoveApplication()
    {
        var request = new RemoveGroupApplicationRequest()
        {
            Group = ek,
        };
        PlayFabGroupsAPI.RemoveGroupApplication(
            request,
            result =>
            {
                Destroy(gameObject);
            }, (e) =>
            {
                SetText(e.ErrorMessage);
            }
        );
    }
}
