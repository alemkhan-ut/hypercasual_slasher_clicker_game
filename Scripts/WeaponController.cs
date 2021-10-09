using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponSpawner_;
    [SerializeField] private GameObject weaponPrefab_;
    [SerializeField] private float moveReactionTime_;

    [SerializeField] private GameObject activeWeapon_;

    private void Start()
    {
        WeaponSpawn();
    }

    public void WeaponSpawn()
    {
        activeWeapon_ = Instantiate(weaponPrefab_, weaponSpawner_);
        activeWeapon_.GetComponent<MiniWeapon>().MoveReactionTime_ = moveReactionTime_;
    }
    public void Attack()
    {
        activeWeapon_.GetComponent<MiniWeapon>().Attack();

        WeaponSpawn();
    }
}
