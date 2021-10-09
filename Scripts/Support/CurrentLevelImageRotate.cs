using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelImageRotate : MonoBehaviour
{
    [SerializeField] [Range(0,5)] private float _currentLevelRotateSpeed = 4f;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0, _currentLevelRotateSpeed));
    }
}
