using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private GameControl gameControl_;
    private EnemySpawner enemySpawner_;
    private BoxCollider2D boxCollider2D_;
    public bool isDestoyer;

    private void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
        enemySpawner_ = FindObjectOfType<EnemySpawner>();
        boxCollider2D_ = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            if (!isDestoyer)
            {
                gameControl_.ShowLosePanel();
                gameControl_.PlayLoseSound();
            }
            else
            {
                enemySpawner_.StopAllCoroutines();
                StartCoroutine(WaitToDisable());
            }

            collision.GetComponent<Enemy>().transform.parent.GetComponent<MovingEnemy>().EnemyDestroy();
        }
    }

    private IEnumerator WaitToDisable()
    {
        yield return new WaitForSeconds(.5f);
        boxCollider2D_.enabled = false;
        gameControl_.CanPlay = true;
        enemySpawner_.StartEnemySpawn();
        gameObject.SetActive(false);
    }
}
