using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float MaxSpeed = 5f;
    private Rigidbody2D rb;
    private bool DoesMove = false;

    private float direction = 1;
    private float distance = 1000f;
    [SerializeField] private float PunchDistance = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent(out rb)) Debug.Log("An Enemy could not find its Rigidbody2D.");
        else DoesMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(target.position.x - transform.position.x);

        if (target.position.x < transform.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }
    private void FixedUpdate()
    {
        // Debug.Log(distance);

        if (DoesMove && distance > PunchDistance)
        {
            rb.linearVelocity = new Vector2(direction * MaxSpeed, rb.linearVelocity.y);
        }
    }
}
