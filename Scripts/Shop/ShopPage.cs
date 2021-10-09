using UnityEngine;

public class ShopPage : MonoBehaviour
{
    private Shop _shop;

    private void Awake()
    {
        if (transform.parent.TryGetComponent(out Shop shop))
        {
            Debug.Log("GET COMPOMEMT TRY");
        }
    }
}
