using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public GameData game_Data;
    public Enemy_Setup enemySetup;
    public Transform transform_;

    [Space]
    [SerializeField] private float absorverForce_;

    [SerializeField] private bool _randomObstacles;
    [SerializeField] private int _minObstacles;
    [SerializeField] private int _maxObstacles;
    [SerializeField] private bool _randomCoins;
    [SerializeField] private int _minCoins;
    [SerializeField] private int _maxCoins;

    public GameObject[] obstacles;
    public GameObject[] coins;
    public GameObject weaponsIn;

    [Header("Property")]
    [SerializeField] private bool isMiniBoss_;
    [SerializeField] private bool isBoss_;
    [SerializeField] private bool isAvailable_;
    [SerializeField] private GameObject availableControl_;
    [SerializeField] private Size size_;
    [SerializeField] private float _health;
    [SerializeField] private float _minHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Text _healthText;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minRotateSpeed;
    [SerializeField] private float _maxRotateSpeed;
    [SerializeField] private float _minTimeToSwitch;
    [SerializeField] private float _maxTimeToSwitch;
    [SerializeField] private Enemy_Setup.Mode _mode; // Его режим/состояние
    [SerializeField] private Enemy_Setup.Mode[] _modes; // Его режим/состояние
    [SerializeField] private float _takeDamage;
    [SerializeField] private float _takeDamageValue;

    [SerializeField] private Sprite[] enemySprites_;
    [SerializeField] private Sprite[] enemyDestroyedSprites_;
    [SerializeField] private RotateHelperImage rotateHelperImage_1;
    [SerializeField] private RotateHelperImage rotateHelperImage_2;

    [SerializeField] private GameObject keyObject_;
    [SerializeField] private float randomValue;

    [SerializeField] private float weaponsTakedAmount_;

    private MovingEnemy movingEnemy_;

    public float Health { get => _health; set => _health = value; }
    public float RotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }
    public Enemy_Setup.Mode[] Modes { get => _modes; set => _modes = value; }
    public WeaponManager WeaponManager_ { get => weaponManager_; set => weaponManager_ = value; }
    public bool IsBoss_ { get => isBoss_; set => isBoss_ = value; }
    public Enemy_Setup.Mode Mode { get => _mode; set => _mode = value; }
    public bool IsAvailable_ { get => isAvailable_; set => isAvailable_ = value; }
    public float WeaponsTakedAmount_ { get => weaponsTakedAmount_; set => weaponsTakedAmount_ = value; }

    private Image _imageComponent;
    private Image enemyDestroyedImage_;
    private BackgroundManager backgroundManager_;
    private GameControl gameControl_;
    private WeaponManager weaponManager_;

    private Collider2D collider2D_1;
    private Collider2D collider2D_2;

    private bool enemyBorn;
    private float beginSpeed;

    private int previousObstacle;
    private int previousCoin;

    private enum Size
    {
        tiny,
        small,
        medium,
        big,
        miniBoss,
        boss
    }


    public void Attacked()
    {
        if (gameControl_.CanPlay)
        {
            if (gameControl_.IsDemo_)
            {
                if (PlayerPrefs.GetInt(gameControl_.game_Data.SelectedWeaponID) == 0)
                {
                    Health -= 20;
                }
                else
                {
                    Health -= 1;
                }
            }
            else
            {
                Health -= 1;
            }
            _healthText.text = ((int)Health).ToString();

            if (Health < 1)
            {
                if (IsBoss_ || isMiniBoss_)
                {
                    if (!gameControl_.IsDemo_)
                    {
                        movingEnemy_.EnemySpawner_.IsBossTime_ = false;
                        movingEnemy_.EnemySpawner_.IsMiniBossTime_ = false;

                        gameControl_.LevelUP();

                        PlayerPrefs.SetInt("TotalBossDestroyed", PlayerPrefs.GetInt("TotalBossDestroyed") + 1);

                        gameControl_.CheckPointHits_ = gameControl_.EnemyDestroyedCount_ + 1;
                        PlayerPrefs.SetInt("CheckPointHits", gameControl_.CheckPointHits_);

                        PlayerPrefs.SetInt(game_Data.CurrentGlobalLevel, gameControl_.CurrentLevelValue_);

                        gameControl_.OpenAfterBossScore();

                        backgroundManager_.BackgroundSwitch();

                        gameControl_.PlayGamePlayMusic();
                        gameControl_.ShowLevelComplete();
                    }
                    else
                    {
                        gameControl_.ShowWinPanel();

                    }
                }

                weaponManager_.target_ = null;

                foreach (var obstacle in obstacles)
                {
                    obstacle.SetActive(false);
                }

                transform.parent.GetComponent<MovingEnemy>().EnemyDestroy(true);
            }
        }
    }

    private IEnumerator EnemyBornAnimation()
    {
        if (isBoss_)
        {
            gameControl_.CanPlay = false;
#if UNITY_EDITOR
            Debug.Log("Мешень появляется. Игра остановлена");
#endif
            transform_.DOScale(0, 0);
            yield return transform_.DOScale(1f, 1f).WaitForCompletion();

            EnemyBorned();
        }
        else
        {
            EnemyBorned();
        }
    }

    public void EnemyBorned()
    {
        collider2D_1.enabled = true;
        collider2D_2.enabled = true;

        if (!gameControl_.LosePanel.activeSelf && !gameControl_.PausePanel.activeSelf && !gameControl_.AfterBossScoreWindow_.activeSelf)
        {
            gameControl_.CanPlay = true;
        }
    }

    private void Awake()
    {
        availableControl_ = FindObjectOfType<AvailableControl>().gameObject;
        transform_ = GetComponent<Transform>();
        _imageComponent = GetComponent<Image>();
        enemyDestroyedImage_ = transform.parent.GetComponent<MovingEnemy>().DestroyedEnemy_.GetComponent<Image>();
        gameControl_ = FindObjectOfType<GameControl>();
        WeaponManager_ = FindObjectOfType<WeaponManager>();
        backgroundManager_ = FindObjectOfType<BackgroundManager>();
        movingEnemy_ = transform.parent.GetComponent<MovingEnemy>();
    }

    private void Start()
    {
        Collider2D[] colliders2D = GetComponents<Collider2D>();

        collider2D_1 = colliders2D[0];
        collider2D_2 = colliders2D[1];

        collider2D_1.enabled = false;
        collider2D_2.enabled = false;

        StartCoroutine(EnemyBornAnimation());

        enemyBorn = true; // Фиксация рождения обьекта для того чтобы при его рождении зарандомить ему поведение движения при старте

        //HideAll(); // Прячем все припятствия
        int randomSpriteIndex = Random.Range(0, enemySprites_.Length);
        _imageComponent.sprite = enemySprites_[randomSpriteIndex];

        RotateSpeed = Random.Range(_minRotateSpeed, _maxRotateSpeed);
        beginSpeed = RotateSpeed;

        int reverseRotation = Random.Range(0, 2);

        if (reverseRotation == 0)
        {
            RotateSpeed = -RotateSpeed;
        }

        _takeDamage = 1f / Health;

        if (gameControl_.CurrentLevelValue_ > 2)
        {
            Mode = Enemy_Setup.Mode.RotateChanging;
        }

        //if (gameControl_.CurrentLevelValue_ < 10)
        //{
        //    if (!isBoss_)
        //    {
        //        _minHealth += gameControl_.CurrentLevelValue_ / 2;
        //        _maxHealth += gameControl_.CurrentLevelValue_ / 2;
        //        _minObstacles += gameControl_.CurrentLevelValue_ / 3;
        //        _maxObstacles += gameControl_.CurrentLevelValue_ / 3;
        //        _minCoins += gameControl_.CurrentLevelValue_ / 3;
        //        _maxCoins += gameControl_.CurrentLevelValue_ / 3;
        //    }
        //    else
        //    {
        //        _minHealth += gameControl_.CurrentLevelValue_ / 2;
        //        _maxHealth += gameControl_.CurrentLevelValue_ / 3;
        //        _minObstacles += gameControl_.CurrentLevelValue_ / 2;
        //        _maxObstacles += gameControl_.CurrentLevelValue_ / 3;
        //        _minCoins += gameControl_.CurrentLevelValue_ / 2;
        //        _maxCoins += gameControl_.CurrentLevelValue_ / 2;
        //    }
        //}
        //else
        //{
        //    if (!isBoss_)
        //    {
        //        _minHealth += 10 / 2;
        //        _maxHealth += 10 / 2;
        //        _minObstacles += 10 / 3;
        //        _maxObstacles += 10 / 3;
        //        _minCoins += 10 / 3;
        //        _maxCoins += 10 / 3;
        //    }
        //    else
        //    {
        //        _minHealth += 10 / 2;
        //        _maxHealth += 10 / 3;
        //        _minObstacles += 10 / 2;
        //        _maxObstacles += 10 / 3;
        //        _minCoins += 10 / 2;
        //        _maxCoins += 10 / 2;
        //    }

        //}

        if (!gameControl_.IsDemo_)
        {
            if (isBoss_)
            {
                if (gameControl_.CurrentLevelValue_ > 3)
                {
                    Mode = Enemy_Setup.Mode.Default;

                    _minHealth += 7;
                    _maxHealth = _maxHealth > 10 ? _maxHealth = 10 : _maxHealth += 7;
                    _minObstacles += 1;
                    _maxObstacles += 1;
                }

                _minHealth += 7;
                _maxHealth += 7;
                _minObstacles *= 2;
                _maxObstacles *= 2;
                _minCoins += 1;
                _maxCoins += 1;
            }
            else
            {

                if (gameControl_.CurrentLevelValue_ <= 2)
                {
                    _minHealth = 3;
                    _maxHealth = 4;
                    _minObstacles = 1;
                    _maxObstacles = 2;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
                else if (gameControl_.CurrentLevelValue_ <= 6)
                {
                    _minHealth = 3;
                    _maxHealth = 5;
                    _minObstacles = 2;
                    _maxObstacles = 2;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
                else if (gameControl_.CurrentLevelValue_ <= 7)
                {
                    _minHealth = 4;
                    _maxHealth = 5;
                    _minObstacles = 2;
                    _maxObstacles = 2;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
                else if (gameControl_.CurrentLevelValue_ <= 11)
                {
                    _minHealth = 5;
                    _maxHealth = 6;
                    _minObstacles = 2;
                    _maxObstacles = 2;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
                else if (gameControl_.CurrentLevelValue_ <= 15)
                {
                    _minHealth = 5;
                    _maxHealth = 7;
                    _minObstacles = 2;
                    _maxObstacles = 3;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
                else if (gameControl_.CurrentLevelValue_ > 15)
                {
                    _minHealth = 5;
                    _maxHealth = 7;
                    _minObstacles = 2;
                    _maxObstacles = 3;
                    _minCoins = 1;
                    _maxCoins = 3;
                }
            }
        }


        Health = (Random.Range(_minHealth, _maxHealth + 1));
        _healthText.text = ((int)Health).ToString();


        GenerateObstacles();
        GenerateCoins();

        keyObject_.SetActive(false);



        randomValue = Random.Range(0f, 100f);
        if (randomValue >= 98f)
        {
            keyObject_.SetActive(true);
        }

        switch (Mode)
        {
            case Enemy_Setup.Mode.Default:
                StartCoroutine(MoveEnemy());
                break;
            case Enemy_Setup.Mode.SpeedChanging:
                StartCoroutine(MoveEnemy("Speed"));
                break;
            case Enemy_Setup.Mode.RotateChanging:
                StartCoroutine(MoveEnemy("Rotate"));
                break;
            case Enemy_Setup.Mode.Breaker:
                StartCoroutine(MoveEnemy("Breaker"));
                break;
            case Enemy_Setup.Mode.Universal:
                StartCoroutine(MoveEnemy("Universal"));
                break;
            default:
                break;
        }

    }

    //public IEnumerator WeaponAbsorver()
    //{
    //    for (int i = 0; i < weaponsIn.transform.childCount; i++)
    //    {
    //        yield return new WaitForSeconds(0.1f);

    //        Weapon weaponInEnemy = weaponsIn.transform.GetChild(i).gameObject.GetComponent<Weapon>();

    //        weaponInEnemy.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    //        weaponInEnemy.Rigidbody2D.AddForce(-weaponInEnemy.transform.up * absorverForce_ * Time.fixedDeltaTime); // даем силу вверх;
    //        weaponInEnemy.enabled = false; // даем силу вверх;
    //        weaponInEnemy.weaponObstacle.SetActive(false); // даем силу вверх;

    //        Destroy(weaponInEnemy.gameObject, 10f);

    //        if (i == weaponsIn.transform.childCount - 1)
    //        {
    //            yield return new WaitForSeconds(3f);
    //            gameControl_.CanPlay = true;
    //        }
    //    }

    //}
    public IEnumerator WeaponAbsorver()
    {
        while (weaponsIn.transform.childCount >= 1)
        {
            for (int i = 0; i < weaponsIn.transform.childCount; i++)
            {
                Weapon weaponInEnemy = weaponsIn.transform.GetChild(i).gameObject.GetComponent<Weapon>();

                weaponInEnemy.enabled = false; // даем силу вверх;
                weaponInEnemy.weaponObstacle.SetActive(false); // даем силу вверх;

                yield return weaponInEnemy.transform.DOLocalMove(Vector3.zero, Random.Range(0.01f, 0.1f)).WaitForCompletion();

                weaponInEnemy.gameObject.SetActive(false);
            }

            for (int i = 0; i < weaponsIn.transform.childCount; i++)
            {
                if (weaponsIn.transform.GetChild(i).gameObject.activeSelf)
                {
                    StartCoroutine(WeaponAbsorver());
                }
            }

            break;
        }

        gameControl_.CanPlay = true;
        yield return transform_.DOScale(transform.localScale.x + 0.25f, 1f).WaitForCompletion();
    }

    private void Update()
    {
        if (transform_.position.y <= availableControl_.transform.position.y)
        {
            IsAvailable_ = true;
        }
    }

    private void HideAll()
    {
        foreach (var obstacle in obstacles) // Перебираем все припятсвтия и прячем
        {
            obstacle.SetActive(false);
        }
        foreach (var coin in coins) // Перебираем все монеты и прячем
        {
            coin.SetActive(false);
        }
    }

    private void GenerateObstacles()
    {
        if (_randomObstacles) // Если включен рандом
        {
            for (int i = 0; i < obstacles.Length; i++) // то мы по длине установленного массива
            {
                obstacles[i].SetActive(false);
            }

            var randomRange = Random.Range(_minObstacles, _maxObstacles + 1); // ПЛЮС ОДИН ДЛЯ EXCLUSIVE

            for (int i = 0; i < randomRange; i++) // то мы по длине установленного массива
            {
                int randomObstacleValue = Random.Range(0, obstacles.Length);

                if (!obstacles[randomObstacleValue].activeSelf)
                {
                    obstacles[randomObstacleValue].SetActive(true); // показываем зарание скрытые случайные обьекты по длине всех других обьектов
                }
            }
        }
    }
    private void GenerateCoins()
    {
        if (_randomCoins) // Если включен рандом
        {
            for (int i = 0; i < coins.Length; i++) // то мы по длине установленного массива
            {
                coins[i].SetActive(false);
                coins[i].SetActive(false);
            }

            var randomRange = Random.Range(_minCoins, _maxCoins + 1); // ПЛЮС ОДИН ДЛЯ EXCLUSIVE

            for (int i = 0; i < randomRange; i++) // то мы по длине установленного массива
            {
                int randomCoinValue = Random.Range(0, coins.Length);

                if (!coins[randomCoinValue].activeSelf)
                {
                    coins[randomCoinValue].SetActive(true); // показываем зарание скрытые случайные обьекты по длине всех других обьектов
                }
            }
        }
    }
    private IEnumerator MoveEnemy()
    {
        while (true)
        {
            if (gameControl_.CanPlay)
            {
                transform_.Rotate(0, 0, RotateSpeed * Time.deltaTime);
            }
            yield return 0;
        }
    }

    private IEnumerator MoveEnemy(string action)
    {
        while (true)
        {
            if (gameControl_.CanPlay)
            {
                if (action == "Speed")
                {
                    if (!IsInvoking("ChangeSpeed") && enemyBorn) // Смена скорости при первом запуске, защита от первых быстрых кликов
                    {
                        Invoke("ChangeSpeed", Random.Range(0, _minTimeToSwitch));

                        enemyBorn = false;
                    }

                    if (!enemyBorn)
                    {
                        transform_.Rotate(0, 0, RotateSpeed * Time.deltaTime);

                        if (!IsInvoking("ChangeSpeed"))
                        {
                            Invoke("ChangeSpeed", Random.Range(_minTimeToSwitch, _maxTimeToSwitch));
                        }
                    }
                }

                if (action == "Rotate")
                {
                    if (!IsInvoking("ChangeRotation") && enemyBorn) // Смена вращения при первом запуске, защита от первых быстрых кликов
                    {
                        float timeToSwitch = Random.Range(0, _minTimeToSwitch);
                        if (isBoss_ && !gameControl_.IsDemo_)
                        {
                            StartCoroutine(SwitchSide(timeToSwitch - 1));
                        }

                        Invoke("ChangeRotation", timeToSwitch);
                        enemyBorn = false;
                    }

                    if (!enemyBorn)
                    {
                        transform_.Rotate(0, 0, RotateSpeed * Time.deltaTime);

                        if (!IsInvoking("ChangeRotation"))
                        {
                            float timeToSwitch = Random.Range(_minTimeToSwitch, _maxTimeToSwitch);
                            if (isBoss_ && !gameControl_.IsDemo_)
                            {
                                StartCoroutine(SwitchSide(timeToSwitch - 1));
                            }

                            Invoke("ChangeRotation", timeToSwitch);
                        }
                    }
                }

                if (action == "Breaker")
                {
                    if (!IsInvoking("SharpBreak") && enemyBorn) // Смена вращения при первом запуске, защита от первых быстрых кликов
                    {
                        Invoke("SharpBreak", Random.Range(0, 2));
                        enemyBorn = false;
                    }


                    if (!enemyBorn)
                    {
                        transform_.Rotate(0, 0, RotateSpeed * Time.deltaTime);

                        if (!IsInvoking("SharpBreak"))
                        {
                            Invoke("SharpBreak", Random.Range(_minTimeToSwitch, _maxTimeToSwitch));
                        }
                    }
                }

                if (action == "Universal")
                {
                    // Не готов
                }
            }
            yield return 0;
        }
    }

    private void ChangeSpeed()
    {
        if (RotateSpeed > 0)
        {
            RotateSpeed = (int)Random.Range(_minRotateSpeed, _maxRotateSpeed);
        }
        else
        {
            RotateSpeed = (int)Random.Range(-_minRotateSpeed, -_maxRotateSpeed);
        }
    }
    private void ChangeRotation()
    {
        RotateSpeed *= -1;
    }
    private void SharpBreak()
    {
        if (Random.Range(0, 2) == 1)
        {
            RotateSpeed = 0;
        }
        else
        {
            RotateSpeed = beginSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Weapon>())
        {
            collision.attachedRigidbody.AddForce(collision.transform.up * collision.GetComponent<Weapon>().speed * 10);
        }
    }
    public IEnumerator SwitchSide(float waitTime = 0)
    {
        yield return new WaitForSeconds(waitTime);

        if (RotateSpeed > 0)
        {
            rotateHelperImage_1.gameObject.SetActive(true);
            rotateHelperImage_2.gameObject.SetActive(false);
        }
        else
        {
            rotateHelperImage_1.gameObject.SetActive(false);
            rotateHelperImage_2.gameObject.SetActive(true);
        }
    }
}

public class EnemyDestroyedObject
{
    public Sprite[] sprite;
}
