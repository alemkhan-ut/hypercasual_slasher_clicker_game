using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    private Transform transform_;
    private GameControl gameControl_;

    [Range(0f, 5f)] [SerializeField] private float moveXSpeed_;

    [SerializeField] private bool isDestroyed_;
    [SerializeField] private GameObject mainEnemy_;
    [SerializeField] private GameObject destroyedEnemy_;
    [SerializeField] private EnemySpawner enemySpawner_;
    [SerializeField] private GameObject healthObject_;

    public GameObject DestroyedEnemy_ { get => destroyedEnemy_; set => destroyedEnemy_ = value; }
    public EnemySpawner EnemySpawner_ { get => enemySpawner_; set => enemySpawner_ = value; }
    public float MoveXSpeed_ { get => moveXSpeed_; set => moveXSpeed_ = value; }
    public GameObject MainEnemy_ { get => mainEnemy_; set => mainEnemy_ = value; }

    private void Awake()
    {
        transform_ = GetComponent<Transform>();
        EnemySpawner_ = FindObjectOfType<EnemySpawner>();
        gameControl_ = FindObjectOfType<GameControl>();
    }

    void Update()
    {
        if (!isDestroyed_ && gameControl_.CanPlay)
        {
            transform_.position -= new Vector3(0, MoveXSpeed_ * Time.deltaTime, 0);
        }
    }

    public void EnemyDestroy(bool isPlayer = false)
    {
        isDestroyed_ = true;
        MainEnemy_.SetActive(false);
        DestroyedEnemy_.SetActive(true);
        healthObject_.SetActive(false);
        EnemySpawner_.RemoveEnemy(gameObject);
        EnemySpawner_.SetLastEnemy();

        if (isPlayer)
        {
            gameControl_.EnemyDestroyed();
        }

        StartCoroutine(DestroyAll());
    }

    private IEnumerator DestroyAll()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
