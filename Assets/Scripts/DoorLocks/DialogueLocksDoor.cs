using UnityEngine;

public class DialogueLocksDoor : MonoBehaviour
{
    [SerializeField] private InteractTransition transition;
    private Dialogue dialogue;

    bool DoesUnlock = false;

    private void Awake()
    {
        if (!TryGetComponent(out dialogue)) Debug.Log("A door lock invocation could not find its respective dialogie component.");
        else DoesUnlock = true;
    }
    private void OnEnable()
    {
        if (DoesUnlock)
        {
            dialogue.OnDialogueDone += FlipLock;
        }
    }
    private void OnDisable()
    {
        if (DoesUnlock)
        {
            dialogue.OnDialogueDone -= FlipLock;
        }
    }

    private void FlipLock()
    {
        if (transition.IsDoorLocked())
        {
            transition.DoorLock(false);
        }
        else
        {
            transition.DoorLock(true);
        }
    }

    
}
