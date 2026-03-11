using System;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance { get; private set; }

    public event Action OnFightStarted;
    public event Action OnFightCompleted;

    [SerializeField] private Dialogue FightDiscussion;
    private int EnemyCount = 0;
    private int DeadEnemyCount = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        FightDiscussion.OnDialogueDone += StartFight;

        foreach(EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            item.OnEnemyDeath += EndFightCheck;
            EnemyCount++;
        }
    }
    private void OnDisable()
    {
        FightDiscussion.OnDialogueDone -= StartFight;

        foreach (EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            item.OnEnemyDeath -= EndFightCheck;
        }
    }

    private void StartFight()
    {
        OnFightStarted.Invoke();
    }
    private void EndFightCheck()
    {
        DeadEnemyCount++;

        if (DeadEnemyCount == EnemyCount)
        {
            OnFightCompleted.Invoke();
        }
    }

}
