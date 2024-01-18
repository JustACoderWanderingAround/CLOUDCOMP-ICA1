using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
public class FriendListDisplay : MonoBehaviour
{
    [SerializeField] private FriendManager friendManager;
    [SerializeField] private Transform itemSpawnPos;
    [SerializeField] GameObject friendRow;
    [SerializeField] private Transform contentTransform;

    private void Start()
    {
        DisplayFriendList();
    }
    public void DisplayFriendList()
    {
        friendManager.GetFriends();
        List<FriendInfo> friendList = friendManager.GetFriendList();

        for (int i = 0; i < friendList.Count; i++)
        {
            Vector3 newItemSpawnPos = new Vector3(itemSpawnPos.position.x, itemSpawnPos.position.y + (i * -210), itemSpawnPos.position.z);
            GameObject oneFriendRow = Instantiate(friendRow, newItemSpawnPos, Quaternion.identity);
            oneFriendRow.GetComponent<FriendRow>().InitRow("Friend "+ i + "name:" + friendList[i].Username, "Tags:" + friendList[i].Tags, friendList[i].FriendPlayFabId);
            oneFriendRow.transform.parent = contentTransform; 
        }
    }
}
