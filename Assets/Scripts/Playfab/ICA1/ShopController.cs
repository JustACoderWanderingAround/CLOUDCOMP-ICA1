using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class ShopController : MonoBehaviour
{
    [SerializeField] private PlayfabCurrencyAndInventoryManager inventoryManager;
    [SerializeField] private GameObject itemBuyUI;
    [SerializeField] private GameObject contentContainer;
    [SerializeField] private TextMeshProUGUI messageBox;
    [SerializeField] private Transform itemSpawnPos;
    [SerializeField] private GameObject canvas;
    void UpdateMessageBox(string message)
    {
        Debug.Log(message);
        messageBox.text = message + "\n";
    }
    private void OnEnable()
    {
        GetCatalog();
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
            int counter = 0;
           
            List<CatalogItem> items = result.Catalog;

            UpdateMessageBox("Catalogue Items: ");
            foreach (CatalogItem i in items)
            {   if (inventoryManager.CheckPresentInPlayerInventory(i.ItemId) == false)
                {
                    Vector3 newItemSpawnPos = new Vector3(itemSpawnPos.position.x, itemSpawnPos.position.y + (counter * -210), itemSpawnPos.position.z);
                    //UpdateMessageBox(i.DisplayName + ", " + i.VirtualCurrencyPrices["CR"]);
                    GameObject oneItemRow = Instantiate(itemBuyUI, newItemSpawnPos, Quaternion.identity);
                    oneItemRow.transform.parent = canvas.transform;
                    uint newItemPrice = 0;
                    i.VirtualCurrencyPrices.TryGetValue("CR", out newItemPrice);
                    int newItemPriceInt = unchecked((int)newItemPrice);
                    oneItemRow.GetComponent<ShopItemBuyRowController>().InitBuyRow(i.ItemId, i.DisplayName, newItemPriceInt, i.Description);
                    counter++;
                } 
            }
        }, OnError
        );
    }

    void OnError(PlayFabError e)
    {
        UpdateMessageBox(e.GenerateErrorReport());
        Debug.Log(e.GenerateErrorReport());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
