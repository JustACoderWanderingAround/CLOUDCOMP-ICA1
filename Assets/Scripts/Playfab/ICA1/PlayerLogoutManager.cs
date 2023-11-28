using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
public class PlayerLogoutManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnLogoutButtonPressed()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene("LoginScene");
    }
}
