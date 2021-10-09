using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class miniEnemy : MonoBehaviour
{
    private Transform transform_;
    private WeaponController weaponController_;
    [SerializeField] private GameObject mainObject_;

    [SerializeField] private float moveSpeed_;
    [SerializeField] private float rotateSpeed_;

    [SerializeField] private int useReverseRotate_;

    [SerializeField] private bool randomObstacles_;
    [SerializeField] private int minObstacles_;
    [SerializeField] private int maxObstacles_;

    public float RotateSpeed_ { get => rotateSpeed_; set => rotateSpeed_ = value; }
    public float MoveSpeed_ { get => moveSpeed_; set => moveSpeed_ = value; }
    public int UseReverseRotate_ { get => useReverseRotate_; set => useReverseRotate_ = value; }
    public bool RandomObstacles_ { get => randomObstacles_; set => randomObstacles_ = value; }
    public int MinObstacles_ { get => minObstacles_; set => minObstacles_ = value; }
    public int MaxObstacles_ { get => maxObstacles_; set => maxObstacles_ = value; }

    private void Awake()
    {
        transform_ = GetComponent<Transform>();
        weaponController_ = FindObjectOfType<WeaponController>();
        ReverseRotate();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        transform_.position -= new Vector3(0, MoveSpeed_ * Time.fixedDeltaTime, 0);
    }

    private void Rotate()
    {
        transform_.Rotate(0, 0, RotateSpeed_, Space.World);
    }

    private void ReverseRotate()
    {
        UseReverseRotate_ = Random.Range(0, 2);

        if (UseReverseRotate_ == 0)
        {
            RotateSpeed_ = -RotateSpeed_;
        }
    }

    public void DestroyMiniEnemy()
    {
        transform_.parent.GetComponent<MiniEnemySpawner>().MiniEnemys_.Remove(gameObject);
        Destroy(gameObject);
    }
}
