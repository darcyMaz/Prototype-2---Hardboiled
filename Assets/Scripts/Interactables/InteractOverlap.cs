using System;
using UnityEngine;

public class InteractOverlap : MonoBehaviour
{
    public event Action OnOverlap;
    public event Action OnOverlapEnd;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") OnOverlap.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") OnOverlapEnd.Invoke();
    }


}
