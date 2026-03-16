using System;
using UnityEngine;

public class EnterFight : MonoBehaviour
{
    public event Action OnQueueIn;

    protected void QueueIn()
    {
        OnQueueIn?.Invoke();
    }
}


