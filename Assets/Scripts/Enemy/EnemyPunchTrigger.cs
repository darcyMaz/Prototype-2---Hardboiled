using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyPunchTrigger : MonoBehaviour
{
    public event Action OnPlayerHit;

    private static List<EnemyPunchTrigger> ept_list = new List<EnemyPunchTrigger>();

    private SpriteRenderer sr;
    private bool UsesSR = false;

    private EnemyMovement ep;

    private Vector2 RightSide;

    private PunchType currentPT;
    private bool IsPunching;

    private void Awake()
    {
        foreach (SpriteRenderer item in GetComponentsInParent<SpriteRenderer>())
        {
            if (item.gameObject.tag == "Enemy") sr = item;
        }
        if (sr == null) { Debug.Log("The PlayerPunchTrigger could not find the SpriteRenderer in the player."); }
        else UsesSR = true;

        ep = GetComponentInParent<EnemyMovement>();
        if (ep == null) { Debug.Log("The PlayerPunchTrigger could not find the PlayerPunch in the player."); }

        ept_list.Add(this);
    }

    private void OnEnable()
    {
        ep.OnPunch += PunchActivated;
        ep.OnPunchComplete += PunchDeactivated;
    }
    private void OnDisable()
    {
        ep.OnPunch -= PunchActivated;
        ep.OnPunchComplete -= PunchDeactivated;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("Inside trigger enemy");

        if (collision.gameObject.tag == "Player" && IsPunching)
        {
            // Debug.Log("Inside trigger enemy player found");
            IsPunching = false;

            PlayerHealth ph;
            if (! collision.gameObject.TryGetComponent(out ph)) Debug.Log("An EnemyPunchTrigger should be able to find the Player's PlayerHealth but can't.");
            else
            {
                ph.PlayerHitTriggered();
            }

            PlayerKnockback pkb;
            if (!collision.gameObject.TryGetComponent(out pkb)) Debug.Log("A PlayerPunchTrigger should be able to find the Enemy's EnemyHealth but can't.");
            else
            {
                float direction = Mathf.Sign(collision.gameObject.transform.position.x - transform.parent.position.x);

                pkb.KnockbackPlayer(currentPT, direction);
            }

            OnPlayerHit?.Invoke();
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


    public static IEnumerable<EnemyPunchTrigger> GetEPTs()
    {
        foreach (EnemyPunchTrigger item in ept_list)
        {
            yield return item;
        }
    }
}
