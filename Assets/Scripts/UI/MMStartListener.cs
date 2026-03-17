using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MMStartListener : MonoBehaviour
{
    private ProjectActions inputActions;
    private InputAction start;

    private void Awake()
    {
        inputActions = new ProjectActions();
    }
    private void OnEnable()
    {
        start = inputActions.Player.Interact;
        start.Enable();

        start.performed += StartGame;
    }
    private void OnDisable()
    {
        start.performed -= StartGame;
        start.Disable();
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.Instance.BufferSceneChange("Outside - Act 1");
    }



}
