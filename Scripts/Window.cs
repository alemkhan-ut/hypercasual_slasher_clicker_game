using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour
{
    [SerializeField] private float startXPosition_;
    [SerializeField] private float targetXPosition_;
    [SerializeField] private float moveDuration_;
    [SerializeField] private bool isAwakeOpen_;

    private Transform transform_;

    private void Start()
    {
        transform_ = GetComponent<Transform>();

        if (isAwakeOpen_)
        {
            OpenWindow();
        }
    }

    public void OpenWindow()
    {
        transform_.DOLocalMoveX(targetXPosition_, moveDuration_).SetEase(Ease.InOutBack);
    }
    public void CloseWindow()
    {
        transform_.DOLocalMoveX(startXPosition_, moveDuration_).SetEase(Ease.InOutBack);
    }
}
