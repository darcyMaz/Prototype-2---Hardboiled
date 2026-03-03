using UnityEngine;

public class DoorUI : MonoBehaviour
{
    private SpriteRenderer ui;
    private bool UseUI = false;
    // Make a script: Door Lock and it does TryGetComponent on DoorUI and then unlocks the door
    [SerializeField] private bool IsDoorLocked = false;
    private bool IsPlayerOverlapping = false;
    [SerializeField] private string NextScene; 

    private void Awake()
    {
        if (!transform.GetChild(0).TryGetComponent(out ui)) Debug.Log("A door could not find its corresponding UI gameobject so it will not appear.");
        else UseUI = true;

        if (UseUI) ui.enabled = false;
    }
    
    private void OnEnable()
    {
        // Subscribe to the player's interact event.
        PlayerInteract.OnInteractPressed += DoorInteract;

        // SceneManager.Instance.OnSceneChange += DoorInteract;
    }

    private void DoorInteract()
    {
        if (IsPlayerOverlapping && !IsDoorLocked)
        {
            // Send the scene string over to the scene manager.
            SceneManager.Instance.BufferSceneChange(NextScene);
        }
    }

    // Show UI that says: press whatever to enter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") ui.enabled = true;
        IsPlayerOverlapping = true;
    }
    
    // Disable UI
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") ui.enabled = false;
        IsPlayerOverlapping = false;
    }
}
