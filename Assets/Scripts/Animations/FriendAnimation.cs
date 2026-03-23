using System;
using UnityEngine;

public class FriendAnimation : MonoBehaviour
{
    private Animator friend_anim;
    private bool HasAnim = false;
    private InteractUI interactUI;
    private bool HasIUI = false;
    private Dialogue friendDialogue;
    private bool HasFriendDialogue = false;
    private SpriteRenderer sr;
    private bool HasSR = false;

    private void Awake()
    {
        if (!TryGetComponent(out friend_anim)) Debug.Log("The FriendAnimation component could not find its Animator component.");
        else HasAnim = true;

        if (!TryGetComponent(out interactUI)) Debug.Log("The FriendAnimation component could not find the player's InteractUI component.");
        else HasIUI = true;

        if (!TryGetComponent(out friendDialogue)) Debug.Log("The FriendAnimation component could not find the player's Dialogue component.");
        else HasFriendDialogue = true;

        if (!TryGetComponent(out sr)) Debug.Log("The SpriteRenderer could not be found on the Friend.");
        else HasSR = true;
    }

    private void OnEnable()
    {
        if (HasAnim && HasIUI && HasFriendDialogue)
        {
            DepTransition.OnDepEntry += StartFriendAnim;
            DepTransition.OnDepExit += EndFriendAnimation;

            friendDialogue.OnDialogueDone += StartExitAnimation;
        }
            
    }
    private void OnDisable()
    {
        if (HasAnim && HasIUI && HasFriendDialogue)
        {
            DepTransition.OnDepEntry -= StartFriendAnim;
            DepTransition.OnDepExit -= EndFriendAnimation;

            friendDialogue.OnDialogueDone -= StartExitAnimation;
        }
    }

    private void StartFriendAnim()
    {
        if (HasSR) sr.flipX = true;

        friend_anim.SetBool("FriendMoves", true);

        if (interactUI != null)
        {
            interactUI.enabled = false;
        }
    }
    private void EndFriendAnimation()
    {
        if (HasSR) sr.flipX = false;

        if (interactUI != null)
        {
            interactUI.enabled = true;
        }
    }

    private void StartExitAnimation()
    {
        if (HasSR) sr.flipX = false;

        friend_anim.SetBool("FriendExit", true);

        if (interactUI != null)
        {
            interactUI.enabled = false;
        }
    }

}
