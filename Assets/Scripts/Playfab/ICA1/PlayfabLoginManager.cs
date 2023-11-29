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
    bool toggle = false;

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
    public void ToggleShowPassword()
    {
        toggle = !toggle;
        if (toggle)
        {
            passwordInputEmail.inputType = TMP_InputField.InputType.Standard;
            passwordInputUsername.inputType = TMP_InputField.InputType.Standard;

        }
        else
        {
            passwordInputEmail.inputType = TMP_InputField.InputType.Password;
            passwordInputUsername.inputType = TMP_InputField.InputType.Password;

        }
        passwordInputEmail.textComponent.SetAllDirty();
        passwordInputUsername.textComponent.SetAllDirty();
    }

}
