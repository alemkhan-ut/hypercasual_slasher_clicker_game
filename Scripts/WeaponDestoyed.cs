using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDestoyed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Weapon>())
        {
            Destroy(collision.GetComponent<Weapon>().gameObject);
            Debug.Log("Уничтожил оружие");
        }
    }
}
