using UnityEngine;

public class LockDialogueDialogueDone : LockDialogue
{
    [SerializeField] private Dialogue dialogue;

    private void OnEnable()
    {
        dialogue.OnDialogueDone += FlipLock;
    }
    private void OnDisable()
    {
        dialogue.OnDialogueDone -= FlipLock;
    }
}
