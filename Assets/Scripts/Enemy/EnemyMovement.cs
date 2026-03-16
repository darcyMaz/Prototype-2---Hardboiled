using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float FarSpeed = 5f;
    [SerializeField] private float NearSpeed = 5f;
    [SerializeField] private float FarDistance = 10f;


    private Rigidbody2D rb;
    private bool DoesMove = false;

    private float direction = 1;
    private float distance = 1000f;
    [SerializeField] private float PunchDistance = 3f;
    [SerializeField] private float PunchDistanceBuffer = 0.5f;

    [SerializeField] private float PunchTime = 0.8f;
    private float PunchTimer = 0;
    private bool PunchReady = false;

    [SerializeField] private float RetractPunchTime = 0.4f;

    [SerializeField] private Sprite punchSprite;
    [SerializeField] private Sprite punchSprite2;
    private Dictionary<PunchType, Sprite> punchSprites = new Dictionary<PunchType, Sprite>();

    [SerializeField] private Sprite readySprite;
    [SerializeField] private BoxCollider2D punchCollider;

    private SpriteRenderer sr;
    private bool UsesSR = false;

    private bool IsFighting = false;
    private bool QueueIn = false;

    private EnemyKnockBack EKB;
    private bool UsesKB = false;

    public event Action <PunchType> OnPunch;
    public event Action OnPunchComplete;

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] float GroundDistance;

    private EnterFight EnterFight;
    private bool CanQueueIn = false;

    private void Awake()
    {
        if (!TryGetComponent(out EKB)) Debug.Log("An enemy could not find its EnemyKnockBack");
        else UsesKB = true;

        if (!TryGetComponent(out sr)) Debug.Log("An Enemy could not find its SpriteRenderer.");
        else UsesSR = true;

        if (!TryGetComponent(out rb)) Debug.Log("An Enemy could not find its Rigidbody2D.");
        else DoesMove = true;

        if (!TryGetComponent(out EnterFight)) Debug.Log("An Enemy could not find its EnterFight.");
        else CanQueueIn = true;

        punchSprites.Add(PunchType.Jab, punchSprite);
        punchSprites.Add(PunchType.Cross, punchSprite2);
    }

    private void OnEnable()
    {
        FightManager.instance.OnFightStarted += StartFight;
        FightManager.instance.OnFightCompleted += EndFight;

        if (UsesKB) EKB.OnEnemyKnockback += StartKnockback;

        if (CanQueueIn) EnterFight.OnQueueIn += queueIn;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightStarted -= StartFight;
        FightManager.instance.OnFightCompleted -= EndFight;

        if (UsesKB) EKB.OnEnemyKnockback -= StartKnockback;

        if (CanQueueIn) EnterFight.OnQueueIn -= queueIn;
    }

    private void StartFight()
    {
        IsFighting = true;
    }
    private void EndFight()
    {
        IsFighting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFighting && QueueIn)
        {
            if (PunchReady && PunchTimer <= 0)
            {
                // Debug.Log("Punch!");
                PunchReady = false;
                Punch();
            }
            PunchTimer = (PunchTimer < 0) ? 0 : PunchTimer - Time.deltaTime;

            distance = Mathf.Abs(target.position.x - transform.position.x);

            if (distance < PunchDistance && !PunchReady)
            {
                // Debug.Log("Close enough to punch. Punch ready.");
                PunchReady = true;
                PunchTimer = PunchTime;
            }
            else if (PunchReady && distance > PunchDistance + PunchDistanceBuffer)
            {
                // Debug.Log("Player outside of punch range. Punch unready.");
                PunchReady = false;
                PunchTimer = 0;
            }

            if (target.position.x < transform.position.x)
            {
                direction = -1;
                sr.flipX = false;
            }
            else
            {
                direction = 1;
                sr.flipX = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsFighting && IsGrounded() && QueueIn)
        {
            if (DoesMove && distance > PunchDistance)
            {
                float CurrentSpeed;

                if (distance > FarDistance) CurrentSpeed = FarSpeed;
                else CurrentSpeed = NearSpeed;

                rb.linearVelocity = new Vector2(direction * CurrentSpeed, rb.linearVelocity.y);
            }
        }
    }

    private void Punch()
    {
        System.Random rand = new System.Random();
        int rand_int = rand.Next(0, punchSprites.Count);

        KeyValuePair<PunchType, Sprite> pair = punchSprites.ElementAt(rand_int);

        // change sprite
        if (UsesSR)
        {
            sr.sprite = pair.Value;
        }

        if (punchCollider != null)
        {
            OnPunch.Invoke(pair.Key);
        }

        Invoke("RetractPunch", RetractPunchTime);
    }
    private void RetractPunch()
    {
        // change sprite
        if (UsesSR) sr.sprite = readySprite;

        if (punchCollider != null)
        {
            OnPunchComplete.Invoke();
        }
    }

    private void StartKnockback(Knockback kb)
    {
        if (UsesKB)
        {
            // give the velocity the values in kb
            rb.linearVelocity = new Vector3(kb.velocity.x, kb.velocity.y, kb.velocity.z);
            IsFighting = false;
        }

        Invoke("EndKnockback", kb.time);
    }

    private void EndKnockback()
    {
        IsFighting = true;
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, GroundDistance, GroundLayer);
    }

    private void queueIn()
    {
        QueueIn = true;
    }
}
