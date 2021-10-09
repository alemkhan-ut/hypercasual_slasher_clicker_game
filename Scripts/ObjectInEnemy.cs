using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInEnemy : MonoBehaviour
{
    public int index;
    public int position;
    private void Start()
    {
        index = transform.GetSiblingIndex();
    }
}
