using UnityEngine;

public class LockedDoorDialogue : MonoBehaviour
{
    InteractTransition it;
    Dialogue d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!TryGetComponent(out d)) Debug.Log("A LockedDoorDialogue component could not find its respective Dialogue component.");
        if (!TryGetComponent(out it)) Debug.Log("A LockedDoorDialogue component could not find its respective InteractTransition component.");
    }

    // This is not good form. Should have an event.
    // Update is called once per frame
    void Update()
    {
        if (it != null && d != null)
        {
            if (it.IsDoorLocked())
            {
                d.enabled = true;
            }
            else
            {
                d.enabled = false;
            }
        }
    }
}
