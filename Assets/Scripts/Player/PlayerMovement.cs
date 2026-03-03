using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool IsRBOn = false;
    private bool IsSROn = false;

    private float direction = 1;

    private ProjectActions projectActions;
    private InputAction move;

    private float CurrentSpeed = 0f;
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private float SmoothTime = 0.1f;

    private float MinSpeed = 0.01f;

    private void Awake()
    {
        projectActions = new ProjectActions();
    }

    private void OnEnable()
    {
        move = projectActions.Player.Move;
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent(out rb)) { Debug.Log("Rigidbody2D could not be found on the player. Disabling the RigidBody2D for this object."); }
        else IsRBOn = true;

        if (!TryGetComponent(out sr)) { Debug.Log("SpriteRenderer could not be found on the player. Disabling the SpriteRenderer for this object."); }
        else IsSROn = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsRBOn) return;
        Move();
        FlipSprite();
    }

    private void Move()
    {
        // Poll for movement: 0 is stationary, 1 is right, -1 is left.
        float movement = move.ReadValue<float>();
        SetDirection(movement);

        // smoothdamp towards but make sure values make it not too noticable
        float speed_ref = 0;

        if (movement != 0)
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, MoveSpeed, ref speed_ref, SmoothTime * Time.fixedDeltaTime);
        else
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, 0, ref speed_ref, SmoothTime * Time.fixedDeltaTime); ;

        if (CurrentSpeed < MinSpeed) CurrentSpeed = 0;

        rb.linearVelocity = new Vector2(direction * CurrentSpeed, rb.linearVelocityY);
    }

    private void SetDirection(float movement)
    {
        direction = (movement != 0) ? movement: direction;
    }

    private void FlipSprite()
    {
        // It would be cool if I could make it flip around like paper mario.
        // Rotate the sprite about the x axis but smoothdamp the rotation


        if (IsSROn) sr.flipX = (direction == 0) ? sr.flipX : (direction == -1) ? true : false;
    }
}
