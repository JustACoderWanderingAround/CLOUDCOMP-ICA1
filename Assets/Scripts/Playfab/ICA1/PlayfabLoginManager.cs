using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfabLoginManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageBox;
    [Header("Registration Fields")]
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInputEmail;
    [SerializeField] TMP_InputField passwordInputUsername;

    IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(3);

        messageBox.text = "WELCOME TO PLAYFAB PAIN SIMULATOR 2023";
    }

    void UpdateMessage(string msg)
    {
        Debug.Log(msg);
        messageBox.text = msg;

    }
    void OnLoginSuccess(LoginResult r)
    {

        UpdateMessage("Login Success" + r.PlayFabId + r.InfoResultPayload.PlayerProfile.DisplayName);
        SceneManager.LoadScene("Menu");
    }
    void OnError(PlayFabError e)
    {
        UpdateMessage("Error" + e.GenerateErrorReport());
    }
    public void OnButtonLoginEmail()
    {
        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInputEmail.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnError);
        SceneManager.LoadScene("Menu");
    }
    public void OnButtonLoginUsername()
    {
        var loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInputUsername.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnError);
        
    }
    public void OnButtonLoginGuest()
    {
        RuntimePlatform deviceRuntimePlatform = Application.platform;

        string CustomUserID = "Guest" + Mathf.Abs(Random.Range(float.MinValue, float.MaxValue)).ToString();

        switch (deviceRuntimePlatform)
        {
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                var loginReq = new LoginWithCustomIDRequest
                {
                    CustomId = CustomUserID,
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithCustomID(loginReq, 
                r=>
                {
                    var customIDUsernameChangeReq = new UpdateUserTitleDisplayNameRequest
                    {
                        DisplayName = CustomUserID
                    };
                    PlayFabClientAPI.UpdateUserTitleDisplayName(customIDUsernameChangeReq, OnDisplayNameUpdate, OnError);
                    SceneManager.LoadScene("Menu");
                }, OnError);
                
                break;
            case RuntimePlatform.Android:
                var androidLoginReq = new LoginWithAndroidDeviceIDRequest
                {
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithAndroidDeviceID(androidLoginReq, r =>
                {
                    var customIDUsernameChangeReq = new UpdateUserTitleDisplayNameRequest
                    {
                        DisplayName = CustomUserID
                    };
                    PlayFabClientAPI.UpdateUserTitleDisplayName(customIDUsernameChangeReq, OnDisplayNameUpdate, OnError);
                    SceneManager.LoadScene("Menu");

                }, OnError);
                break;
            case RuntimePlatform.IPhonePlayer:
                var appleLoginReq = new LoginWithAppleRequest
                {
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithApple(appleLoginReq, r =>
                {
                    var customIDUsernameChangeReq = new UpdateUserTitleDisplayNameRequest
                    {
                        DisplayName = CustomUserID
                    };
                    PlayFabClientAPI.UpdateUserTitleDisplayName(customIDUsernameChangeReq, OnDisplayNameUpdate, OnError);
                    SceneManager.LoadScene("Menu");
                }, OnError);

                break;
            default:
                UpdateMessage("Sorry, your platform not supported for guest login. Please register for an account.");
                break;
        }
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult r)
    {
        UpdateMessage("display name updated!" + r.DisplayName);
    }
    void OnGuestLoginSuccess(LoginResult r)
    {
        UpdateMessage("Registration success!");

       
    }
}
