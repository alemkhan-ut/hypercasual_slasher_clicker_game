using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemySpawner : MonoBehaviour
{
    [SerializeField] Transform transform_;
    [SerializeField] WeaponController weaponController_;

    [SerializeField] private List<GameObject> miniEnemys_;
    [SerializeField] private GameObject firstEnemy_;

    [SerializeField] private float minXSpawnPosition_;
    [SerializeField] private float maxXSpawnPosition_;

    [SerializeField] private GameObject miniEnemyPrefab_;
    [SerializeField] private float respawnDelay_;

    public GameObject FirstEnemy_ { get => firstEnemy_; set => firstEnemy_ = value; }
    public List<GameObject> MiniEnemys_ { get => miniEnemys_; set => miniEnemys_ = value; }

    private void Awake()
    {
        transform_ = GetComponent<Transform>();
        weaponController_ = FindObjectOfType<WeaponController>();
    }

    private void Start()
    {
        StartCoroutine(InstantiateEnemy());
    }

    private IEnumerator InstantiateEnemy()
    {
        GameObject miniEnemy = Instantiate(miniEnemyPrefab_, transform_);
        MiniEnemys_.Add(miniEnemy);

        miniEnemy.transform.localPosition = new Vector3(GetRandomXPosition(), 0, 0);

        yield return new WaitForSeconds(respawnDelay_);
        StartCoroutine(InstantiateEnemy());
    }

    private float GetRandomXPosition()
    {
        return Random.Range(minXSpawnPosition_, maxXSpawnPosition_);
    }

    private void Update()
    {
        //if (MiniEnemys_.Count > 0)
        //{
        //    firstEnemy_ = MiniEnemys_[0];
        //}
    }
}
