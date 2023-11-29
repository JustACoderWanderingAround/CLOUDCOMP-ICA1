using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayfabCurrencyAndInventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageBox;
    [SerializeField] TextMeshProUGUI currencyBox;
    [SerializeField] private GameObject cheatCanvas;
    [SerializeField] private TMP_InputField currencyname;
    [SerializeField] private TMP_InputField currencyAmount;
    int tabPress = 0;
    bool cheatEnabled;
    private int creditCount = 0;
    private int goldCount = 0;
    private List<string> inventoryIDList = new List<string>();
    private void OnEnable()
    {
        GetVirtualCurrencies();
        GetPlayerInventory();
        tabPress = 0;
        cheatEnabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabPress += 1;
        }
        if (cheatCanvas != null)
        {
            if (tabPress >= 5)
            {
                cheatEnabled = true;
            }
            else
            {
                cheatEnabled = false;
            }
            if (cheatCanvas.activeInHierarchy != cheatEnabled)
                cheatCanvas.SetActive(cheatEnabled);
        }

    }
    void UpdateMessageBox(TextMeshProUGUI messageBox, string message)
    {
        Debug.Log(message);
        messageBox.text = message + "\n";
    }
    void UpdateCurrencyBox(int credits)
    {
        currencyBox.text = "CR: " + credits;
    }
    void OnError(PlayFabError e)
    {
        UpdateMessageBox(messageBox, e.GenerateErrorReport());
        Debug.Log(e.GenerateErrorReport());
    }
    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        r =>
        {
            int coins = r.VirtualCurrency["CR"];
            if (creditCount != coins)
            {
                creditCount = coins;
                UpdateCurrencyBox(creditCount);
            }
            
        },
        OnError
        );
    }
    public void GetCatalog()
    {
        var catreq = new GetCatalogItemsRequest
        {
            CatalogVersion = "SpaceShooterCatalog"
        };
        PlayFabClientAPI.GetCatalogItems(catreq,
        result =>
        {
            List<CatalogItem> items = result.Catalog;

            UpdateMessageBox(messageBox, "Catalogue Items: ");
            foreach (CatalogItem i in items)
            {
                UpdateMessageBox(messageBox, i.DisplayName + ", " + i.VirtualCurrencyPrices["CR"]);
            }
        }, OnError
        );
    }
    public void BuyItem(string itemID)
    {
        var buyReq = new PurchaseItemRequest
        {
            CatalogVersion = "SpaceShooterCatalog",
            ItemId = itemID,
            VirtualCurrency ="CR",
            Price = 2
        };
        PlayFabClientAPI.PurchaseItem(buyReq,
            result => { UpdateMessageBox(messageBox,"Bought!"); },
            OnError
        );
    }
    public void GetPlayerInventory()
    {
        var userInv = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInv,
        result =>
        {
            List<ItemInstance> ii = result.Inventory;
            //UpdateMessageBox(messageBox, "Player Inventory");
            inventoryIDList.Clear();
            foreach (ItemInstance i in ii)
            {
                inventoryIDList.Add(i.ItemId);
                //UpdateMessageBox(messageBox, i.DisplayName + ", " + i.ItemId + ", " + i.ItemInstanceId);'
                Debug.Log(i.DisplayName + ", " + i.ItemId + ", " + i.ItemInstanceId);
            }
        },
        OnError
        );
    }
    public void AddCurrency(int moneyAdd, string currencyName)
    {
        var moneyAddReq = new AddUserVirtualCurrencyRequest
        {
            Amount = moneyAdd,
            VirtualCurrency = currencyName
        };
        PlayFabClientAPI.AddUserVirtualCurrency(moneyAddReq,
        result =>
        {
            UpdateMessageBox(currencyBox, currencyName +  ": " + result.Balance.ToString());
        }, OnError);
    }
    public void CheatAddCurrency()
    {
        if (cheatEnabled)
        {
            string currencyValueToAdd = currencyAmount.text;
            AddCurrency(int.Parse(currencyValueToAdd), currencyname.text);
        }
    }
    void OnExeSucc(ExecuteCloudScriptResult r)
    {
        UpdateMessageBox(messageBox,"Response from server:  " + r.FunctionResult.ToString());
    }
    public List<string> GetInventoryIDList()
    {
        GetPlayerInventory();
        return inventoryIDList;
    }

}
