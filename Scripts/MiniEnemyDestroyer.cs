using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Prob>())
        {
            collision.GetComponent<Prob>().MiniEnemy_.DestroyMiniEnemy();
        }
    }
}
