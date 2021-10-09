using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public static PurchaseManager Instanse { set; get; }

    private static IStoreController m_StoreController_;
    private static IExtensionProvider m_StoreExtensionProvider_;
    private GameControl gameControl_;
    private AdsManager adsManager_;

    private void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
        adsManager_ = FindObjectOfType<AdsManager>();
    }

    private void Start()
    {
        
    }

    public void CheckAllProducts()
    {
        Debug.Log("Восстанавливаем покупки");

        CheckProduct("no_ads");
        CheckProduct("weapons1pack");
        CheckProduct("weapons2pack");
    }

    public void CheckProduct(string prodID)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
    StartCoroutine("CheckProductCoroutine", prodID);
#endif
    }

    IEnumerator CheckProductCoroutine(string prodID)
    {
        Product cProduct = m_StoreController_.products.WithID(prodID);
        if (cProduct != null && cProduct.hasReceipt)
        {
            if (prodID == "no_ads")
            {
                gameControl_.adsManager_.NoAds_ = true;
                adsManager_.HideBanner();
            }
            if (prodID == "weapons1pack")
            {
                gameControl_.Weapons1pack_ = true;
            }
            if (prodID == "weapons2pack")
            {
                gameControl_.Weapons2pack_ = true;
            }
        }
        else
        {
            if (prodID == "no_ads")
            {
                gameControl_.adsManager_.NoAds_ = false;
            }
            if (prodID == "weapons1pack")
            {
                gameControl_.Weapons1pack_ = false;
            }
            if (prodID == "weapons2pack")
            {
                gameControl_.Weapons2pack_ = false;
            }
        }

        yield return 0;
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == "10keys") gameControl_.AddKey(10);
        if (product.definition.id == "50keys") gameControl_.AddKey(50);
        if (product.definition.id == "100keys") gameControl_.AddKey(100);
        if (product.definition.id == "2600stars") gameControl_.AddStar(2600);
        if (product.definition.id == "1500stars") gameControl_.AddStar(1500);
        if (product.definition.id == "no_ads")
        {
            gameControl_.adsManager_.NoAds_ = true;
            adsManager_.HideBanner();
#if UNITY_EDITOR
            Debug.Log("Вы купили отключение рекламы: (id)" + product.definition.id);
#endif
        }
        if (product.definition.id == "weapons1pack")
        {
            gameControl_.Weapons1pack_ = true;
#if UNITY_EDITOR
            Debug.Log("Вы купили первый пакет с оружиями: (id)" + product.definition.id);
#endif
        }
        if (product.definition.id == "weapons2pack")
        {
            gameControl_.Weapons2pack_ = true;
#if UNITY_EDITOR
            Debug.Log("Вы купили первый пакет с оружиями: (id)" + product.definition.id);
#endif
        }
    }

    public void OnPurchaseFailure(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Ошибка покупки " + product.definition.id + " по причине: " + reason);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new NotImplementedException();
    }
}