using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

    [SerializeField] PlayfabCurrencyAndInventoryManager inventoryManager;
    [SerializeField] private GameObject startButton;
    GameObject[] characters;
    int index;
    private List<string> playerInv;

    void Start() {
        if (startButton != null)
            startButton.SetActive(false);
        index = PlayerPrefs.GetInt("SelectedCharacter");
        characters = new GameObject[transform.childCount];
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++) {
            characters[i] = transform.GetChild(i).gameObject;
            characters[i].SetActive(false);
        }
        if (characters[index]) {
            characters[index].SetActive(true);
        }
        if (inventoryManager != null)
            playerInv = inventoryManager.GetInventoryIDList();
    }
    ///Todo: enable or disable start button based on whether or not the ship is unlocked
    /// 1. Get ship ID from player control script
    /// 2. Get list of items from inventorystring
    /// 3. check if ship is unlocked based on if id is in string
    /// 4. enable or disable button based on that
    /// bonus: text prompt to go to shop and buy ship
    public void toggleLeft() {
        characters[index].SetActive(false);
        if (index == 0) {
            index = transform.childCount - 1;
        } else {
            index--;
        }
        characters[index].SetActive(true);
        bool currShipUnlocked = checkUnlocked(characters[index]);
        if (startButton.activeInHierarchy != currShipUnlocked)
            startButton.SetActive(currShipUnlocked);

    }

    public void toggleRight() {
        characters[index].SetActive(false);
        if(index == transform.childCount-1){
            index = 0;
        }
        else{
            index++;
        }
        characters[index].SetActive(true);
        bool currShipUnlocked = checkUnlocked(characters[index]);
        if (startButton.activeInHierarchy != currShipUnlocked)
            startButton.SetActive(currShipUnlocked);
    }

    public void selectCharacterAndStart(){
        PlayerPrefs.SetInt("SelectedCharacter", index);
        SceneManager.LoadScene("Main");
    }
    public int getIndex(){
        return index;
    }
   bool checkUnlocked(GameObject gameObject)
    {
        PlayerControl shipData = gameObject.GetComponent<PlayerControl>();
        if (shipData.shipName == "Default")
        {
            return true;
        }
        else
        {
            foreach (string iter in playerInv)
            {
                if (iter == shipData.shipName)
                {
                    return true;
                }
                else
                    continue;
            }
            return false;
        }
    }
}
