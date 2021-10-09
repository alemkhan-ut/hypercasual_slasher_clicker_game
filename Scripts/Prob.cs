using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Prob : MonoBehaviour
{
    private Transform transform_;
    [SerializeField] private miniEnemy miniEnemy_;
    [SerializeField] private GameObject[] obstacles_;

    [SerializeField] private float rotateSpeed_;

    public miniEnemy MiniEnemy_ { get => miniEnemy_; set => miniEnemy_ = value; }

    private void Awake()
    {
        transform_ = GetComponent<Transform>();
    }
    private void Start()
    {
        rotateSpeed_ = MiniEnemy_.RotateSpeed_;
        GenerateObstacles();
    }

    void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        transform_.Rotate(0, 0, rotateSpeed_);
    }
    private void GenerateObstacles()
    {
        if (MiniEnemy_.RandomObstacles_) // Если включен рандом
        {
            var randomRange = Random.Range(MiniEnemy_.MinObstacles_, MiniEnemy_.MaxObstacles_ + 1); // ПЛЮС ОДИН ДЛЯ EXCLUSIVE


            for (int i = 0; i < randomRange; i++) // то мы по длине установленного массива
            {
                int randomObstacleValue = Random.Range(0, obstacles_.Length);

                if (!obstacles_[randomObstacleValue].activeSelf)
                {

                    obstacles_[randomObstacleValue].SetActive(true); // показываем зарание скрытые случайные обьекты по длине всех других обьектов
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
