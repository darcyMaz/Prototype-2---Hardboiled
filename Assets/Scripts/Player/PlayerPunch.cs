using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour
{
    private ProjectActions projActions;
    private InputAction jab;
    private InputAction cross;

    private SpriteRenderer sr;
    private bool UsesSR = false;

    [SerializeField] private Sprite idleFight;
    [SerializeField] private Sprite jabSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite walkSprite;

    // Each punch will have a minimum time extended.
    // You won't be able to turbo punch basically.
    private float PunchTime = 0.4f;
    private float PunchTimer = 0f;
    private bool PunchDone = true;

    [SerializeField] private BoxCollider2D fistColliderL;
    [SerializeField] private BoxCollider2D fistColliderR;

    private bool IsFightActive = false;

    private void Awake()
    {
        projActions = new ProjectActions();
        if (!TryGetComponent(out sr)) Debug.Log("PlayerPunch could not find the player's SpriteRenderer.");
        else UsesSR = true;
    }
    private void OnEnable()
    {
        jab = projActions.Player.Jab;
        cross = projActions.Player.Cross;

        jab.Enable();
        jab.performed += Jab;
        jab.canceled += EndPunch;

        cross.Enable();
        cross.performed += Cross;
        cross.canceled += EndPunch;

        FightManager.instance.OnFightStarted += FightStarted;
        FightManager.instance.OnFightCompleted += FightCompleted;
    }

    private void OnDisable()
    {
        jab.Disable();
        jab.performed -= Jab;
        jab.canceled -= EndPunch;

        cross.Disable();
        cross.performed -= Cross;
        cross.canceled -= EndPunch;

        FightManager.instance.OnFightStarted -= FightStarted;
        FightManager.instance.OnFightCompleted -= FightCompleted;
    }

    private void Update()
    {
        if (IsFightActive)
        {
            if (PunchDone && PunchTimer <= 0) ReadyStance();
            PunchTimer = (PunchTimer <= 0) ? 0 : PunchTimer - Time.deltaTime;
        }
    }

    private void Jab(InputAction.CallbackContext press)
    {
        if (IsFightActive && PunchTimer <= 0)
        {
            EnableFist();
            if (UsesSR) sr.sprite = jabSprite;
            PunchTimer = PunchTime;
            PunchDone = false;
        }
    }
    private void Cross(InputAction.CallbackContext press)
    {
        if (IsFightActive && PunchTimer <= 0)
        {
            EnableFist();
            if (UsesSR) sr.sprite = crossSprite;
            PunchTimer = PunchTime;
            PunchDone = false;
        }
    }

    private void EndPunch(InputAction.CallbackContext release)
    {
        if (IsFightActive) PunchDone = true;
    }

    private void ReadyStance()
    {
        if (IsFightActive && sr.sprite != idleFight)
        {
            if (UsesSR) sr.sprite = idleFight;
        }
    }

    private void EnableFist()
    {
        if (IsFightActive)
        {
            if (sr.flipX) fistColliderL.enabled = true;
            else fistColliderR.enabled = true;

            // Disabling the fist is seperate from the sprite as the fist won't hurt just sitting there in the air.
            Invoke("DisableFists", 0.2f);
        }
    }

    private void DisableFists()
    {
        fistColliderL.enabled = false;
        fistColliderR.enabled = false;
    }

    private void FightStarted()
    {
        IsFightActive = true;
    }
    private void FightCompleted()
    {
        IsFightActive = false;
    }
}
