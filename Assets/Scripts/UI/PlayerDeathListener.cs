using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeathListener : MonoBehaviour
{
    private ProjectActions inputActions;
    private InputAction start;
    private InputAction quit;
    private InputAction resume;


    private void Awake()
    {
        inputActions = new ProjectActions();
    }
    private void OnEnable()
    {
        start = inputActions.UI.Pause;
        start.Enable();
        start.performed += StartGame;

        resume = inputActions.UI.Start;
        resume.Enable();
        resume.performed += ResetLevel;

        quit = inputActions.UI.Quit;
        quit.Enable();
        quit.performed += QuitGame;

    }
    private void OnDisable()
    {
        start.Disable();

        resume.Disable();

        quit.Disable();
    }

    private void ResetLevel(InputAction.CallbackContext context)
    {
        SceneManager.Instance.BufferSceneChange(SceneManager.Instance.GetPreviousSceneName());
    }
    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.Instance.BufferSceneChange("Outside - Act 1");
    }
    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
