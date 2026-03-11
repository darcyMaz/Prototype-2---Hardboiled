using System;
using UnityEngine;

public abstract class FightManagerAid : MonoBehaviour
{
    public event Action OnFightStartedAid;
    public event Action OnFightCompletedAid;

    public abstract void StartFight();
}
