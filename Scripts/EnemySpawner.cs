using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float x_min = -350;
    [SerializeField] private float x_max = 350;

    [SerializeField] private WeaponManager weaponManager_;
    [SerializeField] private GameControl gameControl_;
    [SerializeField] private BackgroundManager backgroundManager_;

    [SerializeField] private bool isMiniBossTime_;
    [SerializeField] private bool isBossTime_;

    [SerializeField] private GameObject[] enemyPrefabs_;
    [SerializeField] private GameObject miniBossEnemyPrefab_;
    [SerializeField] private GameObject[] bossEnemyPrefabs_;

    [SerializeField] private List<GameObject> spawnedEnemys_;
    [SerializeField] private GameObject lastEnemy_;
    [SerializeField] private GameObject boss_;
    [SerializeField] private float spawnTime_;

    public bool IsMiniBossTime_ { get => isMiniBossTime_; set => isMiniBossTime_ = value; }
    public float SpawnTime_ { get => spawnTime_; set => spawnTime_ = value; }
    public bool IsBossTime_ { get => isBossTime_; set => isBossTime_ = value; }

    private void Awake()
    {
        weaponManager_ = FindObjectOfType<WeaponManager>();
        gameControl_ = FindObjectOfType<GameControl>();
        backgroundManager_ = FindObjectOfType<BackgroundManager>();
    }

    public void StartEnemySpawn()
    {
        if (!gameControl_.IsDemo_)
        {
            StartCoroutine(backgroundManager_.BackgroundSwitch());
        }

        StartCoroutine(EnemySpawn());
    }

    public IEnumerator EnemySpawn()
    {
        if (gameControl_.IsDemo_ && gameControl_.IsOnlyDemoBoss_)
        {
            gameControl_.GetBoss();
        }
        else
        {
            if (!IsMiniBossTime_ && !isBossTime_ && gameControl_.CanPlay)
            {

                GameObject enemy = Instantiate(GetRandomEnemy(), transform);
                enemy.transform.localPosition = new Vector3(GetRandomX(), 0, 0);
                spawnedEnemys_.Add(enemy);
                SetLastEnemy();


                enemy.name = "Enemy " + spawnedEnemys_.Count;

                if (isBossTime_ || isMiniBossTime_)
                {
                    Destroy(enemy);
                }

                if (isBossTime_)
                {
                    for (int i = 0; i < spawnedEnemys_.Count; i++)
                    {
                        spawnedEnemys_[i].GetComponent<MovingEnemy>().EnemyDestroy();
                    }
                }

                yield return new WaitForSeconds(SpawnTime_);
                StartCoroutine(EnemySpawn());
            }
            else
            {
                yield return new WaitUntil(() => gameControl_.CanPlay);
                yield return new WaitForSeconds(SpawnTime_);
                StartCoroutine(EnemySpawn());
            }
        }
    }

    public IEnumerator MiniBossEnemySpawn()
    {
        if (!isMiniBossTime_)
        {
            isMiniBossTime_ = true;

            StartCoroutine(backgroundManager_.BackgroundSwitch(true));

            GameObject enemy = Instantiate(miniBossEnemyPrefab_, transform);
            enemy.transform.localPosition = Vector3.zero;

            for (int i = 0; i < spawnedEnemys_.Count; i++)
            {
                spawnedEnemys_[i].GetComponent<MovingEnemy>().EnemyDestroy();
            }


            StartCoroutine(weaponManager_.MoveAnimation(enemy));
        }

        yield return 0;
    }

    public IEnumerator BossEnemySpawn()
    {
        if (boss_ == null)
        {
            if (gameControl_.IsDemo_)
            {
                gameControl_.SoundManager.PlayDemoEvilLaught();
            }

            DestroyBoss();
            DestroyAllSimpleEnemy();

            //if (!isBossTime_)
            //{
            IsBossTime_ = true;

            StartCoroutine(backgroundManager_.BackgroundSwitch(true));

            yield return new WaitForSeconds(.5f);

            boss_ = Instantiate(bossEnemyPrefabs_[Random.Range(0, bossEnemyPrefabs_.Length)], transform);
            boss_.transform.localPosition = new Vector3(0, -1200, 0);

            gameControl_.CanPlay = true;

            StartCoroutine(weaponManager_.MoveAnimation(boss_));
            //}

            yield return 0;
        }
    }

    public void DestroyBoss()
    {
        Destroy(boss_);
        IsBossTime_ = false;
    }

    public void DestroyAllSimpleEnemy()
    {
        for (int i = 0; i < spawnedEnemys_.Count; i++)
        {
            spawnedEnemys_[i].GetComponent<MovingEnemy>().EnemyDestroy();
        }
    }

    private float GetRandomX()
    {
        return Random.Range(x_min, x_max);
    }
    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs_.Length);
        return enemyPrefabs_[randomIndex];
    }

    public void SetLastEnemy()
    {
        if (spawnedEnemys_.Count > 0)
        {
            lastEnemy_ = spawnedEnemys_[0];
        }

        if (lastEnemy_ != null)
        {
            StartCoroutine(weaponManager_.MoveAnimation(lastEnemy_));
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemys_.Remove(enemy);
    }
}
