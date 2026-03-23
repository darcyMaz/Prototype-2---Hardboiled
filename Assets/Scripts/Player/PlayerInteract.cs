using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public static event System.Action OnInteractPressed;

    private ProjectActions projectActions;
    private InputAction interact;

    private bool IsPaused = false;

    private void Awake()
    {
        projectActions = new ProjectActions();
    }
    private void OnEnable()
    {
        interact = projectActions.Player.Interact;
        interact.Enable();
        interact.performed += InteractPressed;

        PauseManager.instance.OnPause += GamePaused;
        PauseManager.instance.OnPauseEnd += GameUnpaused;
    }
    private void OnDisable()
    {
        interact.Disable();

        PauseManager.instance.OnPause -= GamePaused;
        PauseManager.instance.OnPauseEnd -= GameUnpaused;
    }

    private void InteractPressed(InputAction.CallbackContext action)
    {
        if (!IsPaused) OnInteractPressed?.Invoke();
    }

    private void GamePaused()
    {
        IsPaused = true;
    }
    private void GameUnpaused()
    {
        IsPaused = false;
    }
}
