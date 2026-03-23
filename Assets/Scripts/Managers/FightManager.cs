using System;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance { get; private set; }

    public event Action OnFightStarted;
    public event Action OnFightCompleted;
    public event Action OnFightFailed;

    [SerializeField] private Dialogue FightDiscussion;
    private int EnemyCount = 0;
    private int DeadEnemyCount = 0;
    [SerializeField] private PlayerHealth playerHealth;    
    
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        FightDiscussion.OnDialogueDone += StartFight;

        // Check this foreach loop
        foreach(EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            item.OnEnemyDeath += EndFightCheck;
            EnemyCount++;
        }

        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += FightLost;
        }
        else Debug.Log("The FightManager could not find the PlayerHealth component.");
    }

    private void OnDisable()
    {
        FightDiscussion.OnDialogueDone -= StartFight;

        foreach (EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            item.OnEnemyDeath -= EndFightCheck;
        }
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= FightLost;
        }
    }

    private void StartFight()
    {
        OnFightStarted?.Invoke();
    }
    private void EndFightCheck()
    {
        DeadEnemyCount++;

        if (DeadEnemyCount == EnemyCount)
        {
            OnFightCompleted?.Invoke();
        }
    }

    private void FightLost()
    {
        OnFightFailed?.Invoke();
        SceneManager.Instance.BufferSceneChange("Player Death");
    }
}
