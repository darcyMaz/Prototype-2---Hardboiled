using UnityEngine;

public class FightLocksDoor : MonoBehaviour
{
    private InteractTransition transition;
    private bool CanTransition = false;

    private void Awake()
    {
        if (!TryGetComponent(out transition))
        {
            Debug.Log("The FightLockDoor component could not find its connected InteractTransition.");
        }
        else CanTransition = true;
    }
    private void OnEnable()
    {
        if (CanTransition)
        {
            FightManager.instance.OnFightCompleted += FlipLock;
        }
    }
    private void OnDisable()
    {
        if (CanTransition)
        {
            FightManager.instance.OnFightCompleted -= FlipLock;
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
