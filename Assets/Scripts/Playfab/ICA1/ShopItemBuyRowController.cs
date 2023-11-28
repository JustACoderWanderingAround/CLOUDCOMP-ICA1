using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

public class ShopItemBuyRowController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameUI, itemPriceUI;
    string itemID, itemName;
    int itemPrice;
    private int ItemPrice
    {
        get { return itemPrice; }
        set { itemPrice = value;}
    }
    string thisItemID;
    
    /// <summary>
    /// Initialise each buy row. 
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="price"></param>
    public void InitBuyRow(string itemID, string itemName,int price)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemPrice = price;

        itemNameUI.text = itemName;
        itemPriceUI.text = itemPrice.ToString();
    }

    public void BuySelf()
    {
        
        var buyReq = new PurchaseItemRequest
        {
            CatalogVersion = "SpaceShooterCatalog",
            ItemId = itemID,
            VirtualCurrency = "CR",
            Price = itemPrice
        };
        PlayFabClientAPI.PurchaseItem(buyReq,
            result => { Debug.Log(thisItemID + " was bought"); },
            OnError
        );
    }
    void OnError(PlayFabError e)
    {
        //UpdateMessageBox(messageBox, e.GenerateErrorReport());
        Debug.Log(e.GenerateErrorReport());
    }
}
