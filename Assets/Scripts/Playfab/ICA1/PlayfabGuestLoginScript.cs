using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayfabGuestLoginScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageBox;
    void UpdateMessage(string msg)
    {
        Debug.Log(msg);
        messageBox.text = msg;

    }
    void OnLoginSuccess(LoginResult r)
    {

        UpdateMessage("Login Success" + r.PlayFabId);
        Debug.Log("Login success" + r.PlayFabId);
        SceneManager.LoadScene("Menu");
    }
    void OnError(PlayFabError e)
    {
        UpdateMessage("Error" + e.GenerateErrorReport());
    }
    public void OnButtonLoginGuest()
    {
        RuntimePlatform deviceRuntimePlatform = Application.platform;

        switch (deviceRuntimePlatform)
        {
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                var loginReq = new LoginWithCustomIDRequest
                {
                    CustomId = "Guest" + Mathf.Abs(Random.Range(float.MinValue, float.MaxValue)).ToString(),
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithCustomID(loginReq, OnLoginSuccess, OnError);
                break;
            case RuntimePlatform.Android:
                var androidLoginReq = new LoginWithAndroidDeviceIDRequest
                {
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithAndroidDeviceID(androidLoginReq, OnLoginSuccess, OnError);
                break;
            case RuntimePlatform.IPhonePlayer:
                var appleLoginReq = new LoginWithAppleRequest
                {
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithApple(appleLoginReq, OnLoginSuccess, OnError);
                break;
            default:
                UpdateMessage("Sorry, your platform not supported for guest login. Please register for an account.");
                break;
        }
    }
}
