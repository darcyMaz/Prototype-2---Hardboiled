using System;
using UnityEngine;

public class InteractOverlap : MonoBehaviour
{
    public event Action OnOverlap;
    public event Action OnOverlapEnd;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.tag == "Player") OnOverlap.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            try
            {
                OnOverlapEnd.Invoke();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("InteractOverlap tried to invoke an event but it had no subscribers. Likely due to the subscribers being disabled at the closing of a scene.");
            }
            
        }
    }


}
