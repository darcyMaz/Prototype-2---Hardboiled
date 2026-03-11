using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool UsesSR = false;

    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Sprite readySprite;

    private EnemyPunchTrigger ept;
    private bool CanTakeDamage = false;
    [SerializeField] private static float MaxHealth = 5;
    [SerializeField] private float health = MaxHealth;
    [SerializeField] private float hitHealth = 1;

    public event Action OnEnemyDeath;

    private static List<EnemyHealth> enemyHealths = new List<EnemyHealth>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyHealths.Add(this);

        if (!TryGetComponent(out ept)) Debug.Log("An Enemy could not find its EnemyPunchTrigger.");
        else CanTakeDamage = true;
    }

    private void OnEnable()
    {
        ept.OnEnemyHit += EnemyHit;
    }
    private void OnDisable()
    {
        ept.OnEnemyHit -= EnemyHit;
    }

    private void EnemyHit()
    {
        health -= hitHealth;
        if (health <= 0) { EnemyDead(); return; }
        else if (UsesSR)
        {
            sr.sprite = hitSprite;
        }

        Invoke("EnemyHitDone", 0.6f);
    }
    private void EnemyHitDone()
    {
        if (UsesSR) sr.sprite = readySprite;
    }

    private void EnemyDead()
    {
        // Dead enemy sprite: for now just the normal hit sprite
        if (UsesSR)
        {
            sr.sprite = hitSprite;
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
}
