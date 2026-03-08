using UnityEngine;

public class PlayerDep : MonoBehaviour
{
    // Player's components to manipulate
    private SpriteRenderer sr;

    private void Start()
    {
        if (!TryGetComponent(out sr)) Debug.Log("The PlayerDep component could not find the player's sprite renderer.");
    }

    private void OnEnable()
    {
        DepTransition.OnDepEntry += EnterDep;
        DepTransition.OnDepExit += ExitDep;
    }

    private void OnDisable()
    {
        DepTransition.OnDepEntry -= EnterDep;
        DepTransition.OnDepExit -= ExitDep;
    }

    private void EnterDep()
    {
        // Disable sprite renderer
        if (sr != null)
        {
            sr.enabled = false;
        }
    }
    private void ExitDep()
    {
        // Enable sprite renderer
        if (sr != null)
        {
            sr.enabled = true;
        }
    }
}
