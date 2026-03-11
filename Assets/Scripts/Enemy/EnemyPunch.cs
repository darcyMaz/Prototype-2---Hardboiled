using System;
using UnityEngine;

public class EnemyPunchTrigger : MonoBehaviour
{
    public event Action OnEnemyHit;

    private void OnEnable()
    {
        OnEnemyHit += EnemyHit;
    }
    private void OnDisable()
    {
        OnEnemyHit -= EnemyHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFist")
        {
            OnEnemyHit.Invoke();
        }
    }

    private void EnemyHit()
    {
        Debug.Log("The enemy has been hit and will lose health.");
    }
}
