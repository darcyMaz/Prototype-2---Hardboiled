using UnityEngine;

public class InteractUI : MonoBehaviour
{
    private SpriteRenderer ui;
    private bool UseUI = false;
    private InteractOverlap InteractOverlap;

    // Make a script: Door Lock and it does TryGetComponent on DoorUI and then unlocks the door

    private void OnEnable()
    {
        if (!TryGetComponent(out InteractOverlap))
        {
            Debug.Log("A DoorUI object tried to get a DoorOverlap component from the same GameObject.");
        }
        else
        {
            InteractOverlap.OnOverlap += EnableUI;
            InteractOverlap.OnOverlapEnd += DisableUI;
        }
    }

    private void OnDisable()
    {
        if (InteractOverlap != null)
        {
            InteractOverlap.OnOverlap -= EnableUI;
            InteractOverlap.OnOverlapEnd -= DisableUI;
        }
    }

    private void Start()
    {        

        // Cycle through the child GameObjects to find the one that has the InteractPrompt tag.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "InteractPrompt")
            {
                // Make sure the right component exists at that GameObject.
                if (!transform.GetChild(i).TryGetComponent(out ui)) Debug.Log("A door could not find its corresponding UI gameobject so it will not appear.");
                else UseUI = true;

                // End the loop after you've found it.
                break;
            }
        }
        if (UseUI) ui.enabled = false;
    }
   
    private void EnableUI()
    {
        if (UseUI) ui.enabled = true;
    }
    private void DisableUI()
    {
        if (UseUI) ui.enabled = false;
    }

}
