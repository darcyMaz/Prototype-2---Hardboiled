using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndListener : MonoBehaviour
{
    private ProjectActions inputActions;
    private InputAction start;
    private InputAction end;

    private void Awake()
    {
        inputActions = new ProjectActions();
    }
    private void OnEnable()
    {
        start = inputActions.UI.Start;
        start.Enable();
        start.performed += StartGame;

        end = inputActions.UI.Quit;
        end.Enable();
        end.performed += EndGame;
    }
    private void OnDisable()
    {
        start.performed -= StartGame;
        start.Disable();

        end.performed -= EndGame;
        end.Disable();
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.Instance.BufferSceneChange("Start Menu");
    }

    private void EndGame(InputAction.CallbackContext context)
    {
        if (context.performed) Application.Quit();
    }
}
