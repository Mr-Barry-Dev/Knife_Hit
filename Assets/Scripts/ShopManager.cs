using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

// Ye script ek empty GameObject par lagao (e.g. "ShopManager") jo Canvas ke andar ho.
public class ShopManager : MonoBehaviour
{
    [Header("Shop Setup")]
    public List<ShopItemData> allItems;    // Sabhi shop items yahan drag karo
    public ShopItemUI itemPrefab;          // ShopItemUI wala prefab
    public Transform contentParent;        // Grid/Vertical Layout Group wala parent jisme items spawn honge

    [Header("Toggle Group")]
    // Isse guarantee hota hai ki ek time par sirf EK hi checkbox ON rahega.
    // "Allow Switch Off" ko Inspector me TRUE rakho agar user ko "kuch bhi equip na karna" allow karna hai,
    // FALSE rakho agar hamesha ek item equipped hona hi chahiye.
    public ToggleGroup equipToggleGroup;

    [Header("Currency")]
    public int startingCoins = 300;
    public TMP_Text coinsText;

    [Header("Events")]
    public UnityEvent<string> onItemEquipped;   // Baaki game systems isse subscribe kar sakte hain (e.g. character par skin lagana)

    private int coins;
    private HashSet<string> ownedItemIds = new HashSet<string>();
    private string equippedItemId = "";

    private Dictionary<string, ShopItemUI> spawnedUI = new Dictionary<string, ShopItemUI>();

    private const string COINS_KEY = "shop_coins";
    private const string OWNED_KEY_PREFIX = "shop_owned_";
    private const string EQUIPPED_KEY = "shop_equipped";

    private void Start()
    {
        LoadData();
        SpawnShopItems();
        UpdateCoinsUI();
    }

    private void SpawnShopItems()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        spawnedUI.Clear();

        foreach (var item in allItems)
        {
            ShopItemUI ui = Instantiate(itemPrefab, contentParent);
            ui.Setup(item, this);

            // Toggle ko ToggleGroup se jodo taaki sirf ek hi checkbox ON reh sake
            if (ui.equipToggle != null && equipToggleGroup != null)
                ui.equipToggle.group = equipToggleGroup;

            spawnedUI[item.itemId] = ui;
        }
    }

    // ---------------- BUY LOGIC ----------------

    public bool TryBuyItem(ShopItemData item)
    {
        if (IsOwned(item.itemId))
            return false;

        if (coins < item.price)
        {
            Debug.Log("Not enough coins to buy " + item.itemName);
            return false;
        }

        coins -= item.price;
        ownedItemIds.Add(item.itemId);

        UpdateCoinsUI();
        SaveData();

        return true;
    }

    public bool IsOwned(string itemId) => ownedItemIds.Contains(itemId);

    // ---------------- EQUIP LOGIC ----------------

    public void EquipItem(string itemId)
    {
        if (!IsOwned(itemId)) return;

        equippedItemId = itemId;
        onItemEquipped?.Invoke(itemId);

        SaveData();

        // Baaki sabhi panels ka visual refresh karo taaki unka checkbox off dikhe
        RefreshAllUI();
    }

    public void UnequipIfEquipped(string itemId)
    {
        if (equippedItemId == itemId)
        {
            equippedItemId = "";
            SaveData();
            RefreshAllUI();
        }
    }

    public bool IsEquipped(string itemId) => equippedItemId == itemId;

    private void RefreshAllUI()
    {
        foreach (var ui in spawnedUI.Values)
            ui.RefreshVisual();
    }

    // ---------------- COINS ----------------

    private void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = coins.ToString();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinsUI();
        SaveData();
    }

    // ---------------- SAVE / LOAD (PlayerPrefs) ----------------
    // Chhote/simple projects ke liye PlayerPrefs theek hai.
    // Bade projects me JSON file ya proper save system use karna better hoga.

    private void SaveData()
    {
        PlayerPrefs.SetInt(COINS_KEY, coins);
        PlayerPrefs.SetString(EQUIPPED_KEY, equippedItemId);

        foreach (var item in allItems)
        {
            int ownedFlag = ownedItemIds.Contains(item.itemId) ? 1 : 0;
            PlayerPrefs.SetInt(OWNED_KEY_PREFIX + item.itemId, ownedFlag);
        }

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        coins = PlayerPrefs.HasKey(COINS_KEY) ? PlayerPrefs.GetInt(COINS_KEY) : startingCoins;
        equippedItemId = PlayerPrefs.GetString(EQUIPPED_KEY, "");

        ownedItemIds.Clear();
        foreach (var item in allItems)
        {
            int ownedFlag = PlayerPrefs.GetInt(OWNED_KEY_PREFIX + item.itemId, 0);
            if (ownedFlag == 1)
                ownedItemIds.Add(item.itemId);
        }
    }

    // Testing/debug ke liye - sab reset kar dega
    [ContextMenu("Reset Shop Data")]
    public void ResetShopData()
    {
        PlayerPrefs.DeleteKey(COINS_KEY);
        PlayerPrefs.DeleteKey(EQUIPPED_KEY);
        foreach (var item in allItems)
            PlayerPrefs.DeleteKey(OWNED_KEY_PREFIX + item.itemId);

        LoadData();
        SpawnShopItems();
        UpdateCoinsUI();
    }
}