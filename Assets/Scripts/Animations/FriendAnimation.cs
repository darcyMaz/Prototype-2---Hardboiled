using System;
using UnityEngine;

public class FriendAnimation : MonoBehaviour
{
    private Animator friend_anim;
    private InteractUI interactUI;
    private Dialogue friendDialogue;

    private void Awake()
    {
        if (!TryGetComponent(out friend_anim)) Debug.Log("The FriendAnimation component could not find its Animator component.");
        if (!TryGetComponent(out interactUI)) Debug.Log("The FriendAnimation component could not find the player's InteractUI component.");
        if (!TryGetComponent(out friendDialogue)) Debug.Log("The FriendAnimation component could not find the player's Dialogue component.");
    }

    private void OnEnable()
    {
        DepTransition.OnDepEntry += StartFriendAnim;
        DepTransition.OnDepExit += EndFriendAnimation;

        friendDialogue.DialogueDone += StartExitAnimation;
    }
    private void OnDisable()
    {
        DepTransition.OnDepEntry -= StartFriendAnim;
        DepTransition.OnDepExit -= EndFriendAnimation;

        friendDialogue.DialogueDone -= StartExitAnimation;
    }

    private void StartFriendAnim()
    {
        friend_anim.SetBool("FriendMoves", true);

        if (interactUI != null)
        {
            interactUI.enabled = false;
        }
    }
    private void EndFriendAnimation()
    {
        if (interactUI != null)
        {
            interactUI.enabled = true;
        }
    }

    private void StartExitAnimation()
    {
        friend_anim.SetBool("FriendExit", true);

        if (interactUI != null)
        {
            interactUI.enabled = false;
        }
    }

}
