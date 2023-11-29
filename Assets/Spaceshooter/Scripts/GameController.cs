using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;
using PlayFab;

public class GameController : MonoBehaviour {

    public PlayfabPlayerDataManager levelManager;
    public Vector3 positionAsteroid;
    public GameObject asteroid;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public int hazardCount;
    public float startWait;
    public float spawnWait;
    public float waitForWaves;
    public Text scoreText;
    public Text gameOverText;
    public Text restartText;
    public Text mainMenuText;

    private bool restart;
    private bool gameOver;
    private int score;
    private List<GameObject> asteroids;

    private Dictionary<string, string> playerData;

    private void Start() {
        asteroids = new List<GameObject> {
            asteroid,
            asteroid2,
            asteroid3
        };
        gameOverText.text = "";
        restartText.text = "";
        mainMenuText.text = "";
        restart = false;
        gameOver = false;
        score = 0;
        StartCoroutine(spawnWaves());
        updateScore();
    }

    private void Update() {
        if(restart){
            if(Input.GetKey(KeyCode.R)){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } 
            else if(Input.GetKey(KeyCode.Q)){
                SceneManager.LoadScene("Menu");
            }
        }
        if (gameOver) {
            restartText.text = "Press R to restart game";
            mainMenuText.text = "Press Q to go back to main menu";
            restart = true;
        }
    }

    private IEnumerator spawnWaves(){
        yield return new WaitForSeconds(startWait);
        while(true){
            for (int i = 0; i < hazardCount;i++){
                Vector3 position = new Vector3(Random.Range(-positionAsteroid.x, positionAsteroid.x), positionAsteroid.y, positionAsteroid.z);
                Quaternion rotation = Quaternion.identity;
                Instantiate(asteroids[Random.Range(0,3)], position, rotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waitForWaves);
            if(gameOver){
                break;
            }
        }
    }

    public void gameIsOver(){
        SendScore(score);
        RewardCurrency();
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void addScore(int score){
        this.score += score;
        updateScore();
    }

    void updateScore(){
        scoreText.text = "Score:" + score;
    }
    public void SendScore(int score)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Highscore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnSuccessUpdateStat, OnError);
    }
    public void RewardCurrency()
    {
        float moneyAdd;
        int playerLevelInt = 0;
        string playerLevelString = " ";
        playerData.TryGetValue("LV", out playerLevelString);
        int.TryParse(playerLevelString, out playerLevelInt);
        moneyAdd = score * (1 + (playerLevelInt * 0.1f));
        // todo: calculate currency to award to player.
        var moneyAddReq = new AddUserVirtualCurrencyRequest
        {
            Amount = (int)moneyAdd,
            VirtualCurrency = "CR"
        };
        PlayFabClientAPI.AddUserVirtualCurrency(moneyAddReq,
        result =>
        {
           Debug.Log("Successfully rewarded!");
        }, OnError);
    }
    void OnError(PlayFabError r)
    {
        Debug.Log("Error: GController" + r.GenerateErrorReport());
    }
    void OnSuccessUpdateStat(UpdatePlayerStatisticsResult r)
    {
        Debug.Log("Scoreboard update success!");
    }
}
