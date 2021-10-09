using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameData gameData;
    public PurchaseManager purchaseManager;

    public Color weaponDontSelectedColor;
    public Color weaponSelectedColor;
    public Color weaponDontPurchasedColor;
    public Color weaponReadtyToPurchase;

    [SerializeField] private int selectedID;

    public GameObject shopContent;
    public GameObject shopPage;
    public GameObject upgradesContent;
    public GameObject upgradesPage;
    public GameObject WeaponButtonPrefab;

    [SerializeField] private List<GameObject> WeaponButtons = new List<GameObject>();

    public WeaponsList WeaponsList = new WeaponsList();

    private string path;
    private MainMenuController menuController;
    private WeaponManager weaponManager_;
    private GameControl gameControl_;
    private SoundManager sound_Manager;

    private void Start()
    {
        menuController = FindObjectOfType<MainMenuController>();
        sound_Manager = FindObjectOfType<SoundManager>();
        weaponManager_ = FindObjectOfType<WeaponManager>();
        gameControl_ = FindObjectOfType<GameControl>();

        if (shopPage.activeSelf)
        {
            RefreshShop();
        }
    }

    public void RefreshShop()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Weapons.json");
#else
        path = Path.Combine(Application.dataPath, "Weapons.json");
#endif

        if (!File.Exists(path))
        {
            for (int i = 0; i < gameData.TotalWeaponsInGame1; i++)
            {
                WeaponsList.Weapons.Add(new Weapons());

                WeaponsList.Weapons[i].ID = i;
                WeaponsList.Weapons[i].Sprite = "weapon_" + i.ToString();
                WeaponsList.Weapons[i].Name = "Weapon " + i.ToString();
                WeaponsList.Weapons[i].Price = i * 400;
                WeaponsList.Weapons[i].DesiredLevel = -1;
                WeaponsList.Weapons[i].State = WeaponButton.WeaponState.DontSelected;
                WeaponsList.Weapons[i].Status = WeaponButton.WeaponStatus.DontPurchased;
            }


            WeaponsList.Weapons[0].Status = WeaponButton.WeaponStatus.Purchased;
            WeaponsList.Weapons[0].State = WeaponButton.WeaponState.Selected;

            SaveWeaponsData();
        }
        else
        {
            WeaponsList = JsonUtility.FromJson<WeaponsList>(File.ReadAllText(path));
            gameData.TotalWeaponsInGame1 = WeaponsList.Weapons.Count;
        }

        for (int i = 0; i < gameData.TotalWeaponsInGame1; i++)
        {
            WeaponButtons.Add(Instantiate(WeaponButtonPrefab, shopContent.transform));
        }

        for (int i = 0; i < WeaponButtons.Count; i++) // Сбрасываем выделения если есть
        {
            WeaponButtons[i].GetComponent<WeaponButton>().State = WeaponButton.WeaponState.DontSelected;
        }

        for (int i = 0; i < WeaponButtons.Count; i++) // Сбрасываем статусы
        {
            WeaponButtons[i].GetComponent<WeaponButton>().Status = WeaponButton.WeaponStatus.DontPurchased;
        }

        for (int i = 0; i < WeaponButtons.Count; i++)
        {
            WeaponButtons[i].GetComponent<WeaponButton>().State = WeaponsList.Weapons[i].State; // Назначаем значения на сцену с базы
            WeaponButtons[i].GetComponent<WeaponButton>().Status = WeaponsList.Weapons[i].Status;  // Назначаем значения на сцену с базы
        }

        SetWeaponButtonPerameters();
        CheckWeaponStateAndStatus();

    }

    public void SetWeaponButtonPerameters()
    {
        for (int i = 0; i < WeaponButtons.Count; i++)
        {
            WeaponButtons[i].GetComponent<WeaponButton>().name = WeaponsList.Weapons[i].Name;

            WeaponButtons[i].GetComponent<WeaponButton>().weaponImage.sprite = Resources.Load<Sprite>(WeaponsList.Weapons[i].Sprite);

            WeaponButtons[i].GetComponent<WeaponButton>().ID = WeaponsList.Weapons[i].ID;

            WeaponButtons[i].GetComponent<WeaponButton>().price = WeaponsList.Weapons[i].Price;

            WeaponButtons[i].GetComponent<WeaponButton>().desiredLevel = WeaponsList.Weapons[i].DesiredLevel;
        }
    }

    public void CheckWeaponStateAndStatus()
    {
        for (int i = 0; i < WeaponButtons.Count; i++)
        {
            if (WeaponButtons[i].GetComponent<WeaponButton>().desiredLevel > PlayerPrefs.GetInt(gameData.GetBestHits()))
            {
                WeaponButtons[i].GetComponent<WeaponButton>().blockPanel.SetActive(true);
                WeaponButtons[i].GetComponent<WeaponButton>().WeaponUnlocked = false;

            }
            else
            {
                WeaponButtons[i].GetComponent<WeaponButton>().blockPanel.SetActive(false);
                WeaponButtons[i].GetComponent<WeaponButton>().WeaponUnlocked = true;
            }

            if (WeaponButtons[i].GetComponent<WeaponButton>().State == WeaponButton.WeaponState.Selected &&
                WeaponButtons[i].GetComponent<WeaponButton>().Status == WeaponButton.WeaponStatus.Purchased)
            {
                WeaponButtons[i].GetComponent<Image>().color = weaponSelectedColor;
                WeaponButtons[i].GetComponent<WeaponButton>().weaponPricePanel.gameObject.SetActive(false);
            }
            else
            if (WeaponButtons[i].GetComponent<WeaponButton>().State == WeaponButton.WeaponState.DontSelected &&
                WeaponButtons[i].GetComponent<WeaponButton>().Status == WeaponButton.WeaponStatus.Purchased)
            {
                WeaponButtons[i].GetComponent<Image>().color = weaponDontSelectedColor;
                WeaponButtons[i].GetComponent<WeaponButton>().weaponPricePanel.gameObject.SetActive(false);
            }
            else
            if (WeaponButtons[i].GetComponent<WeaponButton>().State == WeaponButton.WeaponState.DontSelected &&
                WeaponButtons[i].GetComponent<WeaponButton>().Status == WeaponButton.WeaponStatus.DontPurchased && PlayerPrefs.GetInt("TotalStars") >= WeaponButtons[i].GetComponent<WeaponButton>().price)
            {
                WeaponButtons[i].GetComponent<Image>().color = weaponReadtyToPurchase;
                WeaponButtons[i].GetComponent<WeaponButton>().weaponPricePanel.gameObject.SetActive(true);
            }
            else
            {
                WeaponButtons[i].GetComponent<Image>().color = weaponDontPurchasedColor;
                WeaponButtons[i].GetComponent<WeaponButton>().weaponPricePanel.gameObject.SetActive(true);
            }
        }

        SaveWeaponsData();
    }

    public void WeaponSelected(int weaponID)
    {
        if (sound_Manager != null)
        {
            sound_Manager.PlayButtonSound();
        }

        if (WeaponButtons[weaponID].GetComponent<WeaponButton>().Status == WeaponButton.WeaponStatus.Purchased)
        {
            for (int i = 0; i < WeaponButtons.Count; i++)
            {
                WeaponButtons[i].GetComponent<WeaponButton>().State = WeaponButton.WeaponState.DontSelected;
                WeaponsList.Weapons[i].State = WeaponButton.WeaponState.DontSelected; // SAVE
            }

            WeaponButtons[weaponID].GetComponent<WeaponButton>().State = WeaponButton.WeaponState.Selected;
            WeaponsList.Weapons[weaponID].State = WeaponButton.WeaponState.Selected; // SAVE

            PlayerPrefs.SetInt(gameData.SelectedWeaponID, weaponID);
        }
        else
        {
            if (WeaponButtons[weaponID].GetComponent<WeaponButton>().WeaponUnlocked)
            {
                if (gameControl_.TotalStarCount_ > WeaponButtons[weaponID].GetComponent<WeaponButton>().price)
                {
                    PlayerPrefs.SetInt("TotalStars", gameControl_.TotalStarCount_ -
                        WeaponButtons[weaponID].GetComponent<WeaponButton>().price);

                    if (sound_Manager != null)
                    {
                        sound_Manager.PurchaseAccept();
                    }

                    gameControl_.UpdateUI();

                    for (int i = 0; i < WeaponButtons.Count; i++)
                    {
                        WeaponButtons[i].GetComponent<WeaponButton>().State = WeaponButton.WeaponState.DontSelected;
                        WeaponsList.Weapons[i].State = WeaponButton.WeaponState.DontSelected; // SAVE
                    }

                    WeaponButtons[weaponID].GetComponent<WeaponButton>().Status = WeaponButton.WeaponStatus.Purchased;
                    WeaponsList.Weapons[weaponID].Status = WeaponButton.WeaponStatus.Purchased; // SAVE

                    WeaponsList.Weapons[weaponID].State = WeaponButton.WeaponState.Selected; // SAVE
                }
                else
                {
                    if (sound_Manager != null)
                    {
                        sound_Manager.PurchaseDenied();
                    }
                }
            }
            else
            {
                if (sound_Manager != null)
                {
                    sound_Manager.PurchaseDenied();
                }
            }
        }

        CheckWeaponStateAndStatus();
    }


    public void SaveWeaponsData()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Weapons.json");
#else
        path = Path.Combine(Application.dataPath, "Weapons.json");
#endif
        File.WriteAllText(path, JsonUtility.ToJson(WeaponsList));

    }
    public void DeleteWeaponData()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Weapons.json");
#else
        path = Path.Combine(Application.dataPath, "Weapons.json");
#endif
        File.Delete(path);
    }
}
