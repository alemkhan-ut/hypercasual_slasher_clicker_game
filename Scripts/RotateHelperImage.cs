using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHelperImage : MonoBehaviour
{
    [SerializeField] private Enemy enemy_;
    [SerializeField] private Transform transform_;
    private void Awake()
    {
        transform_ = GetComponent<Transform>();
    }
}
