using UnityEngine;
public class EnterFightStart : EnterFight
{
    private void OnEnable()
    {
        FightManager.instance.OnFightStarted += QueueIn;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightStarted -= QueueIn;
    }
}