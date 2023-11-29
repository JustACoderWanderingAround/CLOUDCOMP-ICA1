using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;
using PlayFab;

public class GameController : MonoBehaviour {

    public PlayerDataScriptableObject playerDataObject;
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
    [HideInInspector]
    public int meteorCount;

    private bool restart;
    private bool gameOver;
    private int score;
    private int secondsPlayed;
    private float elapsedTime;
    private List<GameObject> asteroids;

    private Dictionary<string, UserDataRecord> playerData;

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
        levelManager.GetUserData();
        meteorCount = 0;
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
        elapsedTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (playerData == null && levelManager.GetDataDict() != null)
        {
            playerData = levelManager.GetDataDict();
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
        RewardPlayer();
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
    public void RewardPlayer()
    {
        float moneyAdd;
        int xpAdd;
        int meteorKillCountTmp;
        int playerLevelInt = 0;
        int seconds;
        int newLevel;
        PlayerStats newStats = playerDataObject.stats;
        playerLevelInt = playerDataObject.stats.level;
        moneyAdd = score * (1 + (playerLevelInt * 0.1f));
        xpAdd = playerDataObject.stats.xp;
        xpAdd += score;
        meteorKillCountTmp = playerDataObject.stats.meteorKillCount;
        seconds = playerDataObject.stats.timePlayed;
        seconds += (int)(elapsedTime);
        newLevel = xpAdd / 100;
        newStats.xp = xpAdd;
        newStats.meteorKillCount = meteorKillCountTmp + meteorCount;
        newStats.timePlayed = seconds;
        newStats.timesPlayed = playerDataObject.stats.timesPlayed + 1;
        newStats.level = newLevel;
        if (score > playerDataObject.stats.highScore)
        {
            newStats.highScore = score;
        }
        playerDataObject.stats = newStats;
        var moneyAddReq = new AddUserVirtualCurrencyRequest
        {
            Amount = (int)moneyAdd,
            VirtualCurrency = "CR"
        };
        PlayFabClientAPI.AddUserVirtualCurrency(moneyAddReq,
        result =>
        {
           Debug.Log("Successfully rewarded currency rewards");
        }, OnError);
        levelManager.SetUserData("META", playerDataObject.SaveToString());
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
