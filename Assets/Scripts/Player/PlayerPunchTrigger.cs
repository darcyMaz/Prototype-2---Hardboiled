using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerPunchTrigger : MonoBehaviour
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
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHealth eh;
            if (!collision.gameObject.TryGetComponent(out eh)) Debug.Log("A PlayerPunchTrigger should be able to find the Enemy's EnemyHealth but can't.");
            else
            {
                eh.EnemyHitTriggered();
            }
            OnEnemyHit.Invoke();
        }
    }

    private void EnemyHit()
    {
        Debug.Log("The enemy has been hit and will lose health.");
    }
}
