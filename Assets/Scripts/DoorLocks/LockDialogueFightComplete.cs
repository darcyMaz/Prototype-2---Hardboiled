using System;
using UnityEngine;

public class LockDialogueFightComplete : LockDialogue
{
    public event Action OnStartScene;

    private void OnEnable()
    {
        FightManager.instance.OnFightCompleted += FlipLock; // This turns the Dialogue on when the fight ends.
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightCompleted -= FlipLock;
    }

    private void Start()
    {
        FlipLock(); // This ensures that the scene starts with the Dialogue off.
    }
}
