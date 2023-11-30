using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class PlayerResetPasswordManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_Text messageBox;
    public void SendResetEmail() {
        var req = new SendAccountRecoveryEmailRequest
        {
            Email = emailField.text,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(req, r => { Debug.Log("Email sent!"); 
            messageBox.text = "Email sent! Head back to the home page and re-login." ; }, OnError);
    }
    void OnError(PlayFabError e)
    {

        Debug.Log(e.GenerateErrorReport());
        messageBox.text = e.GenerateErrorReport();
    }
}
