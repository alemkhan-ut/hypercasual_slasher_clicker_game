using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private Shop shop;

    public string name;
    public Image weaponImage;
    public int ID = 0;
    public int price;
    public int desiredLevel;
    public WeaponState State;
    public WeaponStatus Status;
    public Image weaponPricePanel;
    public Text desiredLevelValueText;
    public GameObject blockPanel;
    public Text weaponPriceValueText;

    private bool weaponSelected;
    private bool weaponPurchased;
    private bool weaponDontPurchased;
    private bool weaponUnlocked;

    public bool WeaponUnlocked { get => weaponUnlocked; set => weaponUnlocked = value; }

    public enum WeaponState
    {
        DontSelected,
        Selected
    }
    public enum WeaponStatus
    {
        DontPurchased,
        Purchased
    }

    private void Awake()
    {
        shop = FindObjectOfType<Shop>();
        image = GetComponent<Image>();

        weaponImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        weaponPricePanel = transform.GetChild(1).gameObject.GetComponent<Image>();
        weaponPriceValueText = transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        weaponPriceValueText.text = price.ToString();
        desiredLevelValueText.text = desiredLevel.ToString() + " lvl";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shop.WeaponSelected(ID);

        if (weaponDontPurchased)
        {
            // purchase
        }
        else if (weaponPurchased)
        {
            // select weapon
        }
    }
}
