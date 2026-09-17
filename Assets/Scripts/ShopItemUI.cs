using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro use kar rahe hain price/name ke liye. Agar normal Text use karna hai to TMP_Text ko UnityEngine.UI.Text se replace kar dena.

// Ye script har item ke PREFAB panel par lagana hai.
// Prefab structure (suggestion):
// Panel (Image)  <-- ye script isi par
//   |- Icon (Image)
//   |- NameText (TMP_Text)
//   |- PriceText (TMP_Text)
//   |- BuyButton (Button)
//   |- EquipToggle (Toggle)   <-- checkbox style toggle, isi ka box use hoga
public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    public Image panelBackground;      // Poore panel ka background image (color change ke liye)
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public Toggle equipToggle;         // Checkbox style toggle (Unity ka built-in Toggle hi checkbox jaisa dikhta hai)

    [Header("Colors")]
    public Color purchasedColor = Color.white;   // Original color jab item khareeda ja chuka ho
    public Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Dark/tinted color jab tak khareeda nahi

    private ShopItemData data;
    private ShopManager manager;

    // ShopManager isko call karke panel setup karega
    public void Setup(ShopItemData itemData, ShopManager shopManager)
    {
        data = itemData;
        manager = shopManager;

        nameText.text = data.itemName;
        priceText.text = data.price.ToString();
        if (iconImage != null && data.icon != null)
            iconImage.sprite = data.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClicked);

        equipToggle.onValueChanged.RemoveAllListeners();
        equipToggle.onValueChanged.AddListener(OnEquipToggled);

        RefreshVisual();
    }

    private void OnBuyClicked()
    {
        manager.TryBuyItem(data);
        RefreshVisual();
    }

    private void OnEquipToggled(bool isOn)
    {
        // Agar item khareeda hi nahi gaya, toggle ko forcefully off rakho
        if (!manager.IsOwned(data.itemId))
        {
            equipToggle.SetIsOnWithoutNotify(false);
            return;
        }

        if (isOn)
        {
            // ToggleGroup khud hi baaki sabko off kar dega,
            // hume bas manager ko batana hai ki ye item equip hua hai
            manager.EquipItem(data.itemId);
        }
        else
        {
            // Agar ToggleGroup "Allow Switch Off" = true hai to user isko manually off bhi kar sakta hai
            manager.UnequipIfEquipped(data.itemId);
        }
    }

    // Panel ka color aur interactability update karta hai
    public void RefreshVisual()
    {
        bool owned = manager.IsOwned(data.itemId);
        bool equipped = manager.IsEquipped(data.itemId);

        // Purchased hone par original color, warna dark/tinted color
        if (panelBackground != null)
            panelBackground.color = owned ? purchasedColor : lockedColor;

        // Khareedne ke baad Buy button disable/hide kar do
        buyButton.gameObject.SetActive(!owned);

        // Equip checkbox sirf tabhi interactable hoga jab item owned ho
        equipToggle.interactable = owned;
        equipToggle.SetIsOnWithoutNotify(equipped);
    }
}