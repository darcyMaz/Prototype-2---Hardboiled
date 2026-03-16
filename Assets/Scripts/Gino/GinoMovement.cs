using UnityEngine;
using UnityEngine.UIElements;

public class GinoMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Rigidbody2D rb;
    [SerializeField] private float BufferDistance = 2f;
    [SerializeField] private float topSpeed = 4f; // This should be the same as the player's speed.
    private float direction = 0;
    [SerializeField] private float smoothDamp = 1f;

    private GinoStartMovement GSM;
    private bool CanFollow = true;
    private bool Follows = true;
    private bool FollowOnDialogue = false;

    private void OnEnable()
    {
        if (FollowOnDialogue)
        {
            GSM.OnGinoStartMovement += LetFollow;
        }
    }
    private void OnDisable()
    {
        if (FollowOnDialogue)
        {
            GSM.OnGinoStartMovement -= LetFollow;
        }
    }

    private void Awake()
    {
        if (target == null)
        {
            Debug.Log("The target was not assigned for the GinoMovement component.");
            CanFollow = false;
        }

        if (!TryGetComponent(out rb))
        {
            Debug.Log("The RigidBody2D could not be found on the GinoMovement component.");
            CanFollow = false;
        }

        if (!TryGetComponent(out GSM))
        {
            Debug.Log("The GinoMovement component does not have a Dialogue connected to it. Movement will be allowed right away.");
            Follows = false;
        }
        else FollowOnDialogue = true;
    }

    private void Update()
    {
        // If player is to the right, -1. If player is to the left, 1. Whatever.
        direction = Mathf.Sign(transform.position.x - target.position.x);
    }

    // Movememnt could be improved with proper smoothdamp, accel decel, whatever.
    private void FixedUpdate()
    {
        if (!CanFollow && !Follows) return;

        // 1) Calculate the buffer point
        //    P---buffer point---G    <-- buffer distance is three dashes, P is the target and G is Gino.
        // 2) If gino is on the opposite side of the buffer point and within the buffer distance: slow down towards buffer point
        //    P---buffer point-G-     <-- within buffer distance, but on the opposite side
        // X) If gino is within the buffer distance: do nothing
        //    P-G-buffer point---

        float buffer_point = target.position.x + (direction * BufferDistance);
        float NewVelocity = 0;

        // Within the buffer distance AND on the opposite side of the buffer point and player
        if (Mathf.Abs(transform.position.x - buffer_point) < BufferDistance)
        {
            // Debug.Log(1);

            if (direction == 1 && buffer_point < transform.position.x || direction == -1 && buffer_point > transform.position.x)
            {
                // Debug.Log(2);
                // slow damp towards buffer point, but towards zero
                NewVelocity = Mathf.MoveTowards(rb.linearVelocityX, 0, smoothDamp * Time.fixedDeltaTime);
            }
        }

        // Opposite side of buffer point but NOT within buffer distance.
        // Might want to reorient if else statements
        else if (Mathf.Abs(transform.position.x - buffer_point) > BufferDistance)
        {
            // Debug.Log(3);
            // Smooth damp towards the buffer point, account for direction
            NewVelocity = Mathf.MoveTowards(rb.linearVelocityX, topSpeed, smoothDamp * Time.fixedDeltaTime);
        }

        rb.linearVelocityX = -direction * NewVelocity;
    }

    private void LetFollow()
    {
        Follows = true;
    }
}
