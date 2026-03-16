using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool UsesSR = false;

    [SerializeField] private static float MaxHealth = 5;
    [SerializeField] private float health = MaxHealth;
    [SerializeField] private float hitHealth = 1;

    private Color hitColor1 = Color.paleVioletRed;

    public event Action OnEnemyDeath;
    public event Action OnEnemyHit;

    private static List<EnemyHealth> enemyHealths = new List<EnemyHealth>();

    void Awake()
    {
        enemyHealths.Add(this);

        if (!TryGetComponent(out sr)) Debug.Log("The EnemyHealth component tried to get its SpriteRenderer but couldn't.");
        else UsesSR = true;
    }

    private void OnEnable()
    {
        OnEnemyHit += EnemyHit;
    }
    private void OnDisable()
    {
        OnEnemyHit -= EnemyHit;
    }

    private void EnemyHit()
    {
        // Debug.Log("Enemy hit inside EnemyHealth.EnemyHit()");

        health -= hitHealth;
        if (health <= 0) { EnemyDead(); return; }
        else if (UsesSR)
        {
            sr.color = hitColor1;
        }

        Invoke("EnemyHitDone", 0.6f);
    }
    private void EnemyHitDone()
    {
        if (UsesSR) sr.color = Color.white;
    }

    private void EnemyDead()
    {
        // Dead enemy sprite: for now just the normal hit sprite
        if (UsesSR)
        {
            sr.color = hitColor1;
        }

        Invoke("DestroyEnemy", 0.7f);
    }
    private void DestroyEnemy()
    {
        try
        {
            OnEnemyDeath.Invoke();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("An Enemy died without subscribers to its OnEnemyDead event. This behaviour may be normal.\n" + e.Message);
        }
        
        Destroy(gameObject);
    }

    public static IEnumerable<EnemyHealth> GetEnemyHealths()
    {
        foreach (EnemyHealth item in enemyHealths)
        {
            yield return item;
        }
    }

    public void EnemyHitTriggered()
    {
        OnEnemyHit.Invoke();
    }
}
