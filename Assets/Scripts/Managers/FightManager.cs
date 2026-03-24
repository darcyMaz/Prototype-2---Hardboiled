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
        Debug.Log("Fight manager awake");
        instance = this;
    }
    private void OnEnable()
    {
        Debug.Log("Fight manager on enable");
        FightDiscussion.OnDialogueDone += StartFight;

        // Check this foreach loop
        foreach(EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            Debug.Log(item.name);
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
        Debug.Log("FightManager OnDisable");

        FightDiscussion.OnDialogueDone -= StartFight;

        foreach (EnemyHealth item in EnemyHealth.GetEnemyHealths())
        {
            item.OnEnemyDeath -= EndFightCheck;
        }

        EnemyCount = 0;
        DeadEnemyCount = 0;
        
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
            EnemyHealth.ClearEnemyHealths();
            OnFightCompleted?.Invoke();
        }
    }

    private void FightLost()
    {
        EnemyHealth.ClearEnemyHealths();
        OnFightFailed?.Invoke();
        SceneManager.Instance.BufferSceneChange("Player Death");
    }
}
