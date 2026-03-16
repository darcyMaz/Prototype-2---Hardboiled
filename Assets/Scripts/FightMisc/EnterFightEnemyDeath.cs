using UnityEngine;
public class EnterFightEnemyDeath : EnterFight
{
    [SerializeField] private EnemyHealth eh;

    private void Awake()
    {
        if (eh == null) Debug.Log("An EnterFightEnemyDeath does not have its EnemyHealth script attached.");
    }
    private void OnEnable()
    {
        if (eh != null) eh.OnEnemyDeath += QueueIn;
    }
    private void OnDisable()
    {
        if (eh != null) eh.OnEnemyDeath -= QueueIn;
    }
}