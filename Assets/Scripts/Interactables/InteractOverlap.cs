using System;
using UnityEngine;

public class InteractOverlap : MonoBehaviour
{
    public event Action OnOverlap;
    public event Action OnOverlapEnd;

    private bool Exited = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.tag == "Player" && Exited)
        {
            OnOverlap?.Invoke();
            Exited = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            Exited = true;
            OnOverlapEnd?.Invoke();
        }
    }
}
