using System;
using UnityEngine;

public class PlayerLockMovement : MonoBehaviour
{
    public event Action OnLockMovement;
    public event Action OnUnlockMovement;

    private void OnEnable()
    {
        // Subscribe to all dialogues
        foreach (Dialogue dialogue in Dialogue.GetEnabledDialogues())
        {
            Debug.Log(dialogue.name);

            // OnMovementLock += dialogue.DialogueDone;
            dialogue.OnDialogueStarted += LockMovement;
            dialogue.OnDialogueClosed += UnlockMovement;
        }
    }
    private void OnDisable()
    {
        // Unsubscribe to all dialogues
        foreach (Dialogue dialogue in Dialogue.GetEnabledDialogues())
        {
            // OnMovementLock += dialogue.DialogueDone;
            dialogue.OnDialogueStarted -= LockMovement;
            dialogue.OnDialogueClosed -= UnlockMovement;
        }
    }

    // Subscribed function locks movememnt.
    private void LockMovement()
    {
        // Debug.Log("Lock movement called as a result of event callback.");
        OnLockMovement.Invoke();
    }
    private void UnlockMovement()
    {
        OnUnlockMovement.Invoke();
    }

}
