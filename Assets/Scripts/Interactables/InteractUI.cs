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

    private void Awake()
    {
        if (!transform.GetChild(0).TryGetComponent(out ui)) Debug.Log("A door could not find its corresponding UI gameobject so it will not appear.");
        else UseUI = true;

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
