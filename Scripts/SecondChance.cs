using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChance : MonoBehaviour
{
    GameControl _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameControl>();
    }

    public void SecondChanceActivated()
    {
    }
}
