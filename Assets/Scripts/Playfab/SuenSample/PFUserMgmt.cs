using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PFUserMgmt : MonoBehaviour
{
    [SerializeField] TMP_Text msgbox;
    [SerializeField] TMP_InputField if_username, if_email, if_password;
    public void OnButtonRegUser()
    {
        var regReq = new RegisterPlayFabUserRequest
        {
            Email = if_email.text,
            Password = if_password.text,
            Username = if_username.text

        };
        PlayFabClientAPI.RegisterPlayFabUser(regReq, OnRegSucc, OnError);

    }
    void OnRegSucc(RegisterPlayFabUserResult r)
    {
        msgbox.text = "Register Success!" + r.PlayFabId;
    }
    void OnError(PlayFabError e)
    {
        msgbox.text = "Error " + e.GenerateErrorReport();  
    }
    public void OnButtonLogin()
    {
        var loginReq = new LoginWithEmailAddressRequest
        {
            Email = if_email.text,
            Password = if_password.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginReq, OnLoginSucc, OnError);

    }
    void OnLoginSucc(LoginResult r)
    {
        msgbox.text = "Login Success! " + r.PlayFabId;
    }
}
