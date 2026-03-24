using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour
{
    private ProjectActions projActions;
    private InputAction jab;
    private InputAction cross;
    private InputAction hook;

    private SpriteRenderer sr;
    private bool UsesSR = false;

    [SerializeField] private Sprite idleFight;
    [SerializeField] private Sprite jabSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite hookSprite;
    [SerializeField] private Sprite walkSprite;

    // Each punch will have a minimum time extended.
    // You won't be able to turbo punch basically.
    [SerializeField] private float PunchTime = 0.4f;
    private float PunchTimer = 0f;
    private bool PunchDone = true;

    // The amount of time that the punch can hurt the enemy.
    [SerializeField] private float PunchLandTime = 0.2f;

    [SerializeField] private BoxCollider2D fistCollider;

    private bool IsFightActive = false;

    public event Action <PunchType> OnPunch;
    public event Action OnPunchComplete;

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
        hook = projActions.Player.Hook;

        jab.Enable();
        jab.performed += Jab;
        jab.canceled += EndPunch;

        cross.Enable();
        cross.performed += Cross;
        cross.canceled += EndPunch;

        hook.Enable();
        hook.performed += Hook;
        hook.canceled += EndPunch;

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

        hook.Disable();
        hook.performed -= Hook;
        hook.canceled -= EndPunch;

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
            EnableFist(PunchType.Jab);
            if (UsesSR) sr.sprite = jabSprite;
            PunchTimer = PunchTime;
            PunchDone = false;
        }
    }
    private void Cross(InputAction.CallbackContext press)
    {
        if (IsFightActive && PunchTimer <= 0)
        {
            EnableFist(PunchType.Cross);
            if (UsesSR) sr.sprite = crossSprite;
            PunchTimer = PunchTime;
            PunchDone = false;
        }
    }
    private void Hook(InputAction.CallbackContext press)
    {
        if (IsFightActive && PunchTimer <= 0)
        {
            EnableFist(PunchType.Hook);
            if (UsesSR) sr.sprite = hookSprite;
            PunchTimer = PunchTime;
            PunchDone = false;
        }
    }
    private void EnableFist(PunchType pt)
    {
        if (IsFightActive)
        {
            OnPunch?.Invoke(pt);

            // Disabling the fist is seperate from the sprite as the fist won't hurt just sitting there in the air.
            Invoke("DisableFists", PunchLandTime);
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

    
    private void DisableFists()
    {
        OnPunchComplete.Invoke();
    }
    

    private void FightStarted()
    {
        IsFightActive = true;
    }
    private void FightCompleted()
    {
        IsFightActive = false;
        // DisableFists();
        if (UsesSR) sr.sprite = walkSprite;
    }
}
