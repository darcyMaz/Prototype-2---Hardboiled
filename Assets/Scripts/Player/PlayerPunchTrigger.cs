using System;
using UnityEngine;

public class PlayerPunchTrigger : MonoBehaviour
{
    public event Action OnEnemyHit;

    private SpriteRenderer sr;
    private bool UsesSR = false;
    private PlayerPunch pp;
    
    private Vector2 RightSide;

    private PunchType currentPT;
    private bool IsPunching;

    private void Awake()
    {

        foreach (SpriteRenderer item in GetComponentsInParent<SpriteRenderer>())
        {
            if (item.gameObject.tag == "Player") sr = item;
        }
        if (sr == null) { Debug.Log("The PlayerPunchTrigger could not find the SpriteRenderer in the player."); }
        else UsesSR = true;

        pp = GetComponentInParent<PlayerPunch>();
        if (pp == null) { Debug.Log("The PlayerPunchTrigger could not find the PlayerPunch in the player."); }
    }

    private void Start()
    {
        if (UsesSR)
        {
            if (sr.flipX)
            {
                RightSide = new Vector2(-transform.localPosition.x, transform.localPosition.y);
            }
            else RightSide = transform.localPosition;
        }
        
    }

    private void Update()
    {
        if (UsesSR)
        {
                if (!sr.flipX)
            {
                transform.localPosition = RightSide;
            }
            else transform.localPosition = new Vector2(-RightSide.x, RightSide.y);
        }
    }

    private void OnEnable()
    {
        pp.OnPunch += PunchActivated;
        pp.OnPunchComplete += PunchDeactivated;

        OnEnemyHit += EnemyHit;
    }
    private void OnDisable()
    {
        pp.OnPunch -= PunchActivated;
        pp.OnPunchComplete -= PunchDeactivated;

        OnEnemyHit -= EnemyHit;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("Trigger is activating");
        if (collision.gameObject.tag == "Enemy" && IsPunching)
        {
            // Turn off the punch on hit so there's only one hit.
            IsPunching = false;

            // Tell the enemy's health to decrement.
            EnemyHealth eh;
            if (!collision.gameObject.TryGetComponent(out eh)) Debug.Log("A PlayerPunchTrigger should be able to find the Enemy's EnemyHealth but can't.");
            else
            {
                eh.EnemyHitTriggered();
            }

            EnemyKnockBack ekb;
            if (!collision.gameObject.TryGetComponent(out ekb)) Debug.Log("A PlayerPunchTrigger should be able to find the Enemy's EnemyHealth but can't.");
            else
            {
                float direction = Mathf.Sign(collision.gameObject.transform.position.x - transform.parent.position.x);
                ekb.KnockbackEnemy(currentPT, direction);
            }


            OnEnemyHit.Invoke();
        }
    }

    private void PunchActivated(PunchType pt)
    {
        IsPunching = true;
        currentPT = pt;
    }
    private void PunchDeactivated()
    {
        IsPunching = false;
    }

    private void EnemyHit() { }
}
