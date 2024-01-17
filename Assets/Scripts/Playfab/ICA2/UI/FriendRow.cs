using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FriendRow : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    public void InitRow(string name)
    {
        nameText.text = name;
    }
}
