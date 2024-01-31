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
    private EntityKey ek;
    [SerializeField] private TMP_Text mainText;
    public void SetText(string newText)
    {
        mainText.text = newText;
    }
    public void SetID(EntityKey ek, bool toggleTick, bool toggleCross)
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
                tick.SetActive(toggleCross);
                SetText(result.GroupName);
            }, (e) =>
            {
                SetText(e.ErrorMessage);
            }
        );
        
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
