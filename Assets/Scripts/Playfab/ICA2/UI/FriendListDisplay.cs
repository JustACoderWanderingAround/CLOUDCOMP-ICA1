using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
// written by paul baquiran 220150y gd2204
public class FriendListDisplay : MonoBehaviour
{
    [SerializeField] private FriendManager friendManager;
    [SerializeField] private Transform itemSpawnPos;
    [SerializeField] GameObject friendRow;
    [SerializeField] private Transform contentTransform;
    private List<GameObject> friendRowlist = new List<GameObject>();
    private void Start()
    {
        DisplayFriendList();
    }
    public void DisplayFriendList()
    {
        if (friendRowlist.Count != 0)
        {
            for (int i = 0; i < friendRowlist.Count; ++i)
            {
                Destroy(friendRowlist[i]);
            }
        }
        friendManager.GetFriends();
        List<FriendInfo> friendList = friendManager.GetFriendList();

        for (int i = 0; i < friendList.Count; i++)
        {
            Vector3 newItemSpawnPos = new Vector3(itemSpawnPos.position.x, itemSpawnPos.position.y + (i * -210), itemSpawnPos.position.z);
            GameObject oneFriendRow = Instantiate(friendRow, newItemSpawnPos, Quaternion.identity);
            string tagText = "";
            if (friendList[i].Tags != null)
            {
                for (int j = 0; j < friendList[i].Tags.Count; j++)
                {
                    if (friendList[i].Tags[j] == "requester")
                        tagText += "Pending friend request";
                    else if (friendList[i].Tags[j] == "requestee")
                        tagText += "Awaiting response";
                    else
                        tagText += friendList[i].Tags[j];
                    tagText += ", ";
                }
            }
            oneFriendRow.GetComponent<FriendRow>().InitRow(i + 1 + " :" + friendList[i].Username, tagText, friendList[i].FriendPlayFabId);
            oneFriendRow.transform.parent = contentTransform;
            friendRowlist.Add(oneFriendRow);
        }
    }
}
