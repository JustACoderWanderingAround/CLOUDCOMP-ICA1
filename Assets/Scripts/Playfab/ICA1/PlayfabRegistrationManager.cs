using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfabRegistrationManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageBox;
    [Header("Registration Fields")]
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField emailConfirmationInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField passwordConfirmationInput;
    [SerializeField] TMP_InputField displayNameInput;

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

    void OnSuccess(RegisterPlayFabUserResult r)
    {
        UpdateMessage("Registration success!");
        if (displayNameInput.text == "")
        {
            displayNameInput.text = usernameInput.text;
        }
        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayNameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnError);
    }

    void OnError(PlayFabError e)
    {
        UpdateMessage("Error" + e.GenerateErrorReport());
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult r)
    {
        UpdateMessage("display name updated!" + r.DisplayName);
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// A function to check if the text entered in 2 fields is the same. 
    /// Used to check email and passwords
    /// </summary>
    /// <param name="textField1">The first text field to compare</param>
    /// <param name="textField2">The second text field to compare</param>
    /// <returns></returns>
    public bool CheckSameText(TMP_InputField textField1, TMP_InputField textField2)
    {

        return textField1.text == textField2.text;
    }
    public void OnRegister()
    {
        if (!CheckSameText(emailInput, emailConfirmationInput) || emailInput.text.Length < 1)
        {
            messageBox.text = "Please re-confirm your email.";
            StartCoroutine(ResetMessage());
            return;
        }
        if (!CheckSameText(passwordInput, passwordConfirmationInput) || passwordInput.text.Length < 1)
        {
            messageBox.text = "Please re-confirm your password.";
            StartCoroutine(ResetMessage());
            return;
        }
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            Username = usernameInput.text
        };
        Debug.Log("Email:" + emailInput.text + "\n" +
            "Password:" + passwordInput.text + "\n" +
            "Username:" + usernameInput.text + "\n"
            );
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnSuccess, OnError);
       
    }
   
}
