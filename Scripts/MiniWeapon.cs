using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniWeapon : MonoBehaviour
{
    private Transform transform_;
    private MiniEnemySpawner miniEnemySpawner_;
    private float targetXPosition_;

    [SerializeField] private bool canAttack_;
    [SerializeField] private float moveReactionTime_;
    [SerializeField] private float attackSpeed_;
    [SerializeField] private bool isThrowed_;

    public float MoveReactionTime_ { get => moveReactionTime_; set => moveReactionTime_ = value; }

    private void Awake()
    {
        transform_ = GetComponent<Transform>();
        miniEnemySpawner_ = FindObjectOfType<MiniEnemySpawner>();

        canAttack_ = true;
    }

    private void Update()
    {
        if (miniEnemySpawner_.FirstEnemy_ != null)
        {
            targetXPosition_ = miniEnemySpawner_.FirstEnemy_.transform.position.x;

        }
    }

    public void Move()
    {
        StartCoroutine(MoveAnimation());
    }

    public IEnumerator MoveAnimation()
    {
        canAttack_ = false;
        yield return transform_.DOMoveX(targetXPosition_, MoveReactionTime_).WaitForCompletion();
        canAttack_ = true;
    }
    public void Attack()
    {
        if (canAttack_)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Prob>())
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Prob>().MiniEnemy_.DestroyMiniEnemy();
        }

        if (collision.gameObject.GetComponent<ObjectInEnemy>())
        {
            Destroy(gameObject);
        }
    }
}
