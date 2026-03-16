using System;
using UnityEngine;

public class LockDialogue : MonoBehaviour
{
    public event Action OnFlipLockDialogue;
    protected void FlipLock()
    {
        OnFlipLockDialogue?.Invoke();
    }
}
