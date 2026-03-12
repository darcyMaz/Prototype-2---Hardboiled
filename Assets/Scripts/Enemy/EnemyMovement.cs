using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float MaxSpeed = 5f;
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
    [SerializeField] private Sprite readySprite;
    [SerializeField] private BoxCollider2D punchColliderRight;
    [SerializeField] private BoxCollider2D punchColliderLeft;

    private SpriteRenderer sr;
    private bool UsesSR = false;

    private bool IsFighting = false;

    private void OnEnable()
    {
        FightManager.instance.OnFightStarted += StartFight;
        FightManager.instance.OnFightCompleted += EndFight;
    }
    private void OnDisable()
    {
        FightManager.instance.OnFightStarted -= StartFight;
        FightManager.instance.OnFightCompleted -= EndFight;
    }

    private void StartFight()
    {
        IsFighting = true;
    }
    private void EndFight()
    {
        IsFighting = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent(out sr)) Debug.Log("An Enemy could not find its SpriteRenderer.");
        else UsesSR = true;

        if (!TryGetComponent(out rb)) Debug.Log("An Enemy could not find its Rigidbody2D.");
        else DoesMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFighting)
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
        if (IsFighting)
        {
            if (DoesMove && distance > PunchDistance)
            {
                rb.linearVelocity = new Vector2(direction * MaxSpeed, rb.linearVelocity.y);
            }
        }
    }

    private void Punch()
    {
        System.Random rand = new System.Random();
        int rand_int = rand.Next(0, 2);

        // change sprite
        if (UsesSR)
        {
            if (rand_int == 0) sr.sprite = punchSprite;
            else sr.sprite = punchSprite2;
        }
        
        // activate trigger zone
        if (punchColliderRight != null && direction == 1) punchColliderRight.enabled = true;
        else if (direction == -1 && punchColliderLeft != null) punchColliderLeft.enabled = true;

        Invoke("RetractPunch", RetractPunchTime);
    }
    private void RetractPunch()
    {
        // change sprite
        if (UsesSR) sr.sprite = readySprite;

        // activate trigger zone
        if (punchColliderRight != null && direction == 1) punchColliderRight.enabled = false;
        else if (direction == -1 && punchColliderLeft != null) punchColliderLeft.enabled = false;
    }
}
