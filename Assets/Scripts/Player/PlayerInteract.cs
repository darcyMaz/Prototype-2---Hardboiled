using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public static event System.Action OnInteractPressed;

    private ProjectActions projectActions;
    private InputAction interact;

    private void Awake()
    {
        projectActions = new ProjectActions();
    }
    private void OnEnable()
    {
        interact = projectActions.Player.Interact;
        interact.Enable();
        interact.performed += InteractPressed;
    }
    private void OnDisable()
    {
        interact.Disable();
    }

    private void InteractPressed(InputAction.CallbackContext action)
    {
        OnInteractPressed.Invoke();
    }
}
