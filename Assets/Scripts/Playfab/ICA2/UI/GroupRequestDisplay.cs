using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.GroupsModels;
// written by paul baquiran 220150y gd2204
public class GroupRequestDisplay : MonoBehaviour
{
    private List<GroupApplication> groupApplications;
    private List<GroupInvitation> groupInvitation;
    public GameObject inviteDisplaySpawnPos;
    public GameObject inviteDisplayRowPrefab;
    public GameObject applicationDisplaySpawnPos;
    public GameObject applicationDisplayRowPrefab;
    private void Awake()
    {
        GetMembershipOpportunities();
    }
    private void OnSharedError(PlayFab.PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    public void GetMembershipOpportunities()
    {
        Debug.Log("gotten membership opps");
        PlayFabGroupsAPI.ListMembershipOpportunities(new ListMembershipOpportunitiesRequest(),
            result =>
            {
                groupApplications = result.Applications;
                groupInvitation = result.Invitations;
                DisplayMembershipOpportunities();
            }, OnSharedError
            );
    }
    public void DisplayMembershipOpportunities()
    {
        for (int i = 0; i < groupApplications.Count; ++i)
        {
            Vector3 newItemSpawnPos = new Vector3(applicationDisplaySpawnPos.transform.position.x, applicationDisplaySpawnPos.transform.position.y + (i * -210), applicationDisplaySpawnPos.transform.position.z);
            GameObject oneInviteRow = Instantiate(applicationDisplayRowPrefab, newItemSpawnPos, Quaternion.identity);
            oneInviteRow.GetComponent<GroupRow>().SetID(groupApplications[i].Group, false, true);
            oneInviteRow.transform.parent = applicationDisplaySpawnPos.transform;
        }
        for (int i = 0; i < groupInvitation.Count; ++i)
        {
            Vector3 newItemSpawnPos = new Vector3(inviteDisplaySpawnPos.transform.position.x, inviteDisplaySpawnPos.transform.position.y + (i * -210), inviteDisplaySpawnPos.transform.position.z);
            GameObject oneInviteRow = Instantiate(inviteDisplayRowPrefab, newItemSpawnPos, Quaternion.identity);
            oneInviteRow.GetComponent<GroupRow>().SetID(groupInvitation[i].Group, true, true);
            oneInviteRow.transform.parent = inviteDisplaySpawnPos.transform;
        }
    }
}
