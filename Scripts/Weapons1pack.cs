using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons1pack : MonoBehaviour
{
    private GameControl gameControl_;

    void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl_.Weapons1pack_)
        {
            gameObject.SetActive(false);
        }
    }
}
