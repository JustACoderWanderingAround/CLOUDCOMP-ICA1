using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.GroupsModels;
using TMPro;
// written by paul baquiran 220150y gd2204
public class GuildMembershipDetails : MonoBehaviour
{
    //[SerializeField] private ToggleCanvas toggleCanvas;
    [SerializeField] private GameObject fleetList;
    [SerializeField] private GameObject memberList;
    [SerializeField] private GameObject rowPrefab;
    private EntityKey ek;
    private void Awake()
    {
        GetMemberships();
    }
    private void OnSharedError(PlayFab.PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    public void GetMemberships()
    {
        Debug.Log("memberships gotten");
        var request = new ListMembershipRequest();
        PlayFabGroupsAPI.ListMembership(request,
            r =>
            {
                DisplayMembership(r);
            }, OnSharedError
            );
    }
    public void DisplayMembership(ListMembershipResponse r)
    {
        for (int i = 0; i < fleetList.transform.childCount; ++i)
        {
            Destroy(fleetList.transform.GetChild(i));
        }
        List<GroupWithRoles> responseGrps = r.Groups;
        for (int i = 0; i < responseGrps.Count; ++i)
        {
            Vector3 newItemSpawnPos = new Vector3(fleetList.transform.position.x, fleetList.transform.position.y + (i * -210), fleetList.transform.position.z);
            GameObject oneInviteRow = Instantiate(rowPrefab, newItemSpawnPos, Quaternion.identity);
            string newString = responseGrps[i].GroupName + "\nID: " + responseGrps[i].Group.Id;
            if (responseGrps[i].Roles.Count > 0)
            {
                newString += "\nRoles:";
                for (int j = 0; j < responseGrps[i].Roles.Count; ++j)
                {
                    newString += responseGrps[i].Roles[j];
                }
            }
            oneInviteRow.GetComponent<GroupRow>().SetID(responseGrps[i].Group, false, false, true);
            //oneInviteRow.GetComponent<GroupRow>().SetText(newString);
            //oneInviteRow.GetComponent<GroupRow>().viewButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            EntityKey temp = responseGrps[i].Group;
            oneInviteRow.GetComponent<GroupRow>().viewButton.onClick.AddListener(() => GetGroupMembership(temp));
            //oneInviteRow.GetComponent<GroupRow>().gameObject.GetComponentInChildren<TMPro.TMP_Text>().SetText("View group info");
            oneInviteRow.transform.parent = fleetList.transform;
        }
    }
    public void GetGroupMembership(EntityKey ek)
    {
        var request = new ListGroupMembersRequest() { Group = ek };
        PlayFabGroupsAPI.ListGroupMembers(request, r =>
        {
            DisplayGroupMembers(r);
        }, OnSharedError
            );
    }
    public void DisplayGroupMembers(ListGroupMembersResponse r)
    {
        for (int i = 0; i < memberList.transform.childCount; ++i)
        {
            Destroy(memberList.transform.GetChild(i).gameObject);
        }
        List<EntityMemberRole> members = r.Members;
        int counter = 0;
        for (int i = 0; i < members.Count; ++i)
        {
            for (int j = 0; j < members[i].Members.Count; ++j)
            {
                Vector3 newItemSpawnPos = new Vector3(memberList.transform.position.x, memberList.transform.position.y + (counter * -210), memberList.transform.position.z);
                counter++;
                GameObject oneInviteRow = Instantiate(rowPrefab, newItemSpawnPos, Quaternion.identity);
                oneInviteRow.GetComponent<GroupRow>().SetUserID(members[i].Members[j].Key, members[i].RoleName);
                oneInviteRow.transform.parent = memberList.transform;
            }
        }
    }
}
