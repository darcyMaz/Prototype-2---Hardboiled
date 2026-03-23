using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    private ProjectActions inputActions;
    private InputAction pause;
    private InputAction quit;

    public event Action OnPause;
    public event Action OnPauseEnd;

    private bool IsPaused = false;

    private void Awake()
    {
        // Double check this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;


        inputActions = new ProjectActions();
    }

    private void OnEnable()
    {
        pause = inputActions.UI.Pause;
        pause.Enable();
        pause.performed += PauseMenu;

        quit = inputActions.UI.Quit;
        quit.Enable();
        quit.performed += QuitGame;
    }

    private void OnDisable()
    {
        pause.Disable();
        pause.performed -= PauseMenu;

        quit.Disable();
        quit.performed -= QuitGame;
    }

    private void PauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!IsPaused)
            {
                OnPause?.Invoke();
                IsPaused = true;
                Time.timeScale = 0;
            }
            else if (IsPaused)
            {
                OnPauseEnd?.Invoke();
                IsPaused = false;
                Time.timeScale = 1;
            }
        }
    }

    private void QuitGame(InputAction.CallbackContext context)
    {
        if (context.performed && IsPaused)
        {
            Application.Quit();
        }
    }
}
