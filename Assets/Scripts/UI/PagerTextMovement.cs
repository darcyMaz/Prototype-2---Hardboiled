using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PagerTextMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    // These values should be based on the dynamic size of the text so the sentence in the puzzle can change size if needed.
    [SerializeField] float MaxX, MinX;
    private bool CanMove = false;

    private ProjectActions inputActions;
    private InputAction move;

    [SerializeField] private float SmoothTime = 0.5f;
    [SerializeField] private float MaxSpeed = 1f;

    private void Awake()
    {
        inputActions = new ProjectActions();
    }
    private void OnEnable()
    {
        move = inputActions.Pager.Move;
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
    }

    void Start()
    {
        
        if (!TryGetComponent(out rb)) Debug.Log("The PagerButton script cannot find the Rigidbody2D.");
        else CanMove = true;
    }

    private void Update()
    {
        Debug.Log(transform.position.x);

        float currentMove = move.ReadValue<float>();

        if (CanMove)
        {
            float newSpeed = 0f;

            if (currentMove == 1 && transform.position.x > MinX) // 520
            {
                newSpeed = Mathf.MoveTowards(rb.linearVelocityX, -MaxSpeed, SmoothTime * Time.deltaTime);
            }
            else if (currentMove == -1 && transform.position.x < MaxX) // 935
            {
                newSpeed = Mathf.MoveTowards(rb.linearVelocityX, MaxSpeed, SmoothTime * Time.deltaTime);
            }

            rb.linearVelocityX = newSpeed;
        }
    }
}
