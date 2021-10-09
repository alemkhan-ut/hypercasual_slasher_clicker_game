using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    private AllUI allUI_;
    private EnemySpawner enemySpawner_;

    private Rigidbody2D _rigidbody2D;
    private PolygonCollider2D polygonCollider2D_;
    private BoxCollider2D boxCollider2D_;
    private Weapon weapon_;
    private Image image_;
    private GameControl gameControl_;
    private bool _move;
    private bool isFall;
    private Transform transform_;

    public WeaponManager weaponManager;
    public GameObject weaponObstacle;
    public float speed;
    public float fallSpeed;
    public float raycastDistance_;
    public bool _isConnect;
    public bool _isLose;

    [Header("Animations")]
    public float yAnimationFallPosition_;

    public object SceneManagment { get; private set; }
    public Rigidbody2D Rigidbody2D { get => _rigidbody2D; set => _rigidbody2D = value; }

    private void Awake()
    {
        weapon_ = GetComponent<Weapon>();
        transform_ = GetComponent<Transform>();
        allUI_ = FindObjectOfType<AllUI>();
        image_ = GetComponent<Image>();

        Rigidbody2D = GetComponent<Rigidbody2D>();
        polygonCollider2D_ = GetComponent<PolygonCollider2D>();
        boxCollider2D_ = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        gameControl_ = FindObjectOfType<GameControl>();

        weaponManager = (WeaponManager)FindObjectOfType(typeof(WeaponManager));
        enemySpawner_ = FindObjectOfType<EnemySpawner>();
    }
    private IEnumerator WeaponMove()
    {
        while (!_isConnect && _move && gameControl_.CanPlay)
        {
            Rigidbody2D.AddForce(Vector2.up * speed * Time.fixedDeltaTime); // даем силу вверх

            yield return 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Coin>() && _move) // Если касаемся монетки
        {
            gameControl_.AddStarInSession();
            Destroy(collision.gameObject);
        }
        else
        if (collision.GetComponent<Key>() && _move) // Если касаемся монетки
        {
            gameControl_.AddKey();
            Destroy(collision.gameObject);
        }
        else
        if (collision.GetComponent<Enemy>() && _move && gameControl_.CanPlay) // Если мы косаемся врага и мы в это время движемся
        {
            Enemy collisionEnemy = collision.gameObject.GetComponent<Enemy>();

            gameObject.transform.SetParent(collision.gameObject.transform.GetChild(2).transform); // мы назначаем оружие дочерним врага
            weaponObstacle.SetActive(true);
            collisionEnemy.Attacked(); // Вызываем атаку

            collisionEnemy.WeaponsTakedAmount_ += 1;

#if UNITY_EDITOR
            Debug.Log("Ножей уже воткнуто: " + collisionEnemy.WeaponsTakedAmount_);
#endif

            if (gameControl_.IsDemo_)
            {
                if (collisionEnemy.WeaponsTakedAmount_ >= 20)
                {
                    gameControl_.CanPlay = false;
                    collisionEnemy.WeaponsTakedAmount_ = 0;
                    StartCoroutine(collisionEnemy.WeaponAbsorver());
                }
            }

            gameControl_.Hit();
            gameControl_.PlayHitSound();

            _isConnect = true; // Оружие подсоединено
            _move = false; // мы больше не двигаемся

            weaponManager.isWeaponFly = false; // оружие не в полёте

            Rigidbody2D.bodyType = RigidbodyType2D.Static; // Делаем его статик
        }
        else
        if (collision.GetComponent<LoseTriggerForWeapon>()) // Если мы  касаемся проигрышнего триггера
        {
            gameObject.transform.SetParent(gameControl_.transform);
            Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            polygonCollider2D_.enabled = false;
            boxCollider2D_.enabled = false;

            gameControl_.PlayMisstakeHitSound(); // запускаем звук промаха
            _isLose = true;

            gameControl_.CanPlay = false;
#if UNITY_EDITOR
            Debug.Log("Оружие попало в припятствие. Игра остановлена");
#endif
            isFall = true;
            StartCoroutine(Fall());
        }
    }

    public void MoveUp(float speed)
    {
        _move = true;
        StartCoroutine(WeaponMove());

        gameControl_.PlayThrowWeaponSound(); // метод для возпроизведения звука

        this.speed = speed; // назначаем скорость от параметра

        weaponManager.isWeaponFly = true; // указываем что мы оружие в полёте
    }

    public IEnumerator Fall()
    {
        transform_.DOLocalMove(new Vector3(Random.Range(-700, 700), -1200, 0), 2f);
        gameControl_.CanPlay = false;
        transform_.DORotate(new Vector3(0, 0, 800), Random.Range(0.5f, 1f)).WaitForCompletion();
        yield return image_.DOFade(0, .5f).WaitForCompletion();

        gameControl_.ShowLosePanel();

        Destroy(gameObject);
    }

}
