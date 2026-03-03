using System;
using UnityEngine;

public class InteractTransition : MonoBehaviour
{
    [SerializeField] private string NextScene;
    [SerializeField] private bool IsDoorLocked = false;
    private bool IsPlayerOverlapping = false;
    private InteractOverlap InteractOverlap;

    private void OnEnable()
    {
        // Subscribe to the player's interact event.
        PlayerInteract.OnInteractPressed += DoorInteract;

        // Subscribe to InteractOverlap's overap event.
        if (!TryGetComponent(out InteractOverlap))
        {
            Debug.Log("An InteractTransition object tried to get an InteractOverlap component from the same GameObject but failed.");
        }
        else
        {
            InteractOverlap.OnOverlap += ReadyTransition;
            InteractOverlap.OnOverlapEnd += UnreadyTransition;
        }
    }
    private void OnDisable()
    {
        PlayerInteract.OnInteractPressed -= DoorInteract;

        if (InteractOverlap != null)
        {
            InteractOverlap.OnOverlap -= ReadyTransition;
            InteractOverlap.OnOverlapEnd -= UnreadyTransition;
        }
    }

    private void ReadyTransition()
    {
        IsPlayerOverlapping = true;
    }
    private void UnreadyTransition()
    {
        IsPlayerOverlapping = false;
    }

    private void DoorInteract()
    {
        if (IsPlayerOverlapping && !IsDoorLocked)
        {
            // Send the scene string over to the scene manager.
            SceneManager.Instance.BufferSceneChange(NextScene);
        }
    }
}
