using System;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyPunchTrigger : MonoBehaviour
{
    public event Action OnPlayerHit;

    private void OnEnable()
    {
        OnPlayerHit += PlayerHit;
    }
    private void OnDisable()
    {
        OnPlayerHit -= PlayerHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth ph;
            if (! collision.gameObject.TryGetComponent(out ph)) Debug.Log("An EnemyPunchTrigger should be able to find the Player's PlayerHealth but can't.");
            else
            {
                ph.PlayerHitTriggered();
            }
            OnPlayerHit.Invoke();
        }
    }

    private void PlayerHit()
    {
        Debug.Log("The player has been hit and will lose health.");
    }
}
