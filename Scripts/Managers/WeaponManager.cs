using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponManager : MonoBehaviour
{
    public GameObject _weaponInScene;

    public EnemySpawner enemySpawner_;
    public GameControl gameControl_;
    public GameObject target_;

    [Header("Игровая сцена")]
    public GameObject weaponPrefabs;
    public Sprite weaponDefaultSprite;
    public Sprite[] weaponSprite;

    private Sprite selectedWeaponSprite;

    public float attackTime; // время для возможности следующий атаки
    [Range(1f, 20f)] public float speed;
    public bool isWeaponFly; // Оружие в полете
    public bool isMoveEnd;
    public float respawnTime;

    public float weaponMoveTime_;
    private void Awake()
    {
        enemySpawner_ = FindObjectOfType<EnemySpawner>();
        gameControl_ = FindObjectOfType<GameControl>();
    }

    public void SetSelectedWeapon()
    {
        if (PlayerPrefs.HasKey("SelectedWeaponID"))
        {
            for (int i = 0; i < weaponSprite.Length; i++)
            {
                if (weaponSprite[i].name == "weapon_" + PlayerPrefs.GetInt("SelectedWeaponID").ToString())
                {
                    selectedWeaponSprite = weaponSprite[i];
                    break;
                }

                selectedWeaponSprite = weaponDefaultSprite;
            }
        }
        else
        {
            selectedWeaponSprite = weaponDefaultSprite;
        }

        weaponPrefabs.GetComponent<Image>().sprite = selectedWeaponSprite;

        StartCoroutine(WeaponRespawn());
    }

    public void AttackButton() // кнопка на экране для атаки
    {
        if (target_ != null)
        {
            if (gameControl_.CanPlay &&
                target_.GetComponent<MovingEnemy>().MainEnemy_.GetComponent<Enemy>().IsAvailable_)
            {
                if (!IsInvoking("Attack"))
                {
                    Invoke("Attack", attackTime);
                }
            }
        }
    }
    public void Attack()
    {
        if (_weaponInScene != null)
        {
            _weaponInScene.GetComponent<PolygonCollider2D>().enabled = true;
            _weaponInScene.GetComponent<Weapon>().MoveUp(speed); // отправляем оружие в атаку с определённой скоростью
            _weaponInScene = null;
        }
    }

    private IEnumerator WeaponRespawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => _weaponInScene == null);
            yield return new WaitForSeconds(respawnTime);
            _weaponInScene = Instantiate(weaponPrefabs, gameObject.transform);
        }
    }

    private void DestroyAll()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++) // смотрим все дочерние обьекты при их наличии удаляем всё
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void WeaponLeave()
    {
        _weaponInScene.GetComponent<Animator>().SetTrigger("NextLevel");
#if UNITY_EDITOR
        Debug.Log("АНИМАЦИЯ СЫГРАЛА");
#endif
    }

    public IEnumerator MoveAnimation(GameObject target)
    {
        target_ = target;
        yield return transform.DOMoveX(target.transform.position.x, weaponMoveTime_).WaitForCompletion();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackButton();
        }
    }
}
