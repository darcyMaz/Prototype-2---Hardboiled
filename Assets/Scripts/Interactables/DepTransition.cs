using System;
using UnityEngine;

public class DepTransition : MonoBehaviour
{
    [SerializeField] private bool IsDoorLocked = false;
    private bool IsPlayerOverlapping = false;
    private InteractOverlap InteractOverlap;

    public static event System.Action OnDepEntry;
    public static event System.Action OnDepExit;

    private bool DepTimerStarted = false;
    private float DepTimer = 0f;
    [SerializeField] private float DepTime;

    private void OnEnable()
    {
        // Subscribe to the player's interact event.
        PlayerInteract.OnInteractPressed += DepInteract;
        OnDepEntry += EnterDep;
        OnDepExit += ExitDep;

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
        PlayerInteract.OnInteractPressed -= DepInteract;
        OnDepEntry -= EnterDep;
        OnDepExit -= ExitDep;

        if (InteractOverlap != null)
        {
            InteractOverlap.OnOverlap -= ReadyTransition;
            InteractOverlap.OnOverlapEnd -= UnreadyTransition;
        }
    }

    private void Update()
    {
        if (DepTimerStarted && DepTimer <= 0)
        {
            OnDepExit?.Invoke();
        }
        DepTimer = (DepTimer < 0) ? 0 : DepTimer - Time.deltaTime;
    }

    private void ReadyTransition()
    {
        IsPlayerOverlapping = true;
    }
    private void UnreadyTransition()
    {
        IsPlayerOverlapping = false;
    }

    private void DepInteract()
    {
        if (IsPlayerOverlapping && !IsDoorLocked)
        {
            OnDepEntry?.Invoke();
        }
    }

    private void EnterDep()
    {
        DepTimerStarted = true;
        DepTimer = DepTime;
    }

    private void ExitDep()
    {
        IsDoorLocked = true;
        DepTimerStarted = false;
    }
}
