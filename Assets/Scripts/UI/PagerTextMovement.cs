using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PagerTextMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float MaxX, MaxY, MinX, MinY;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent(out rb)) Debug.Log("The PagerButton script cannot find the Rigidbody2D.");
        else CanMove = true;
    }

    private void Update()
    {
        float currentMove = move.ReadValue<float>();

        if (CanMove)
        {
            float newSpeed = 0f;

            if (currentMove == 1)
            {
                newSpeed = Mathf.MoveTowards(rb.linearVelocityX, MaxSpeed, SmoothTime * Time.deltaTime);
            }
            else if (currentMove == -1)
            {
                newSpeed = Mathf.MoveTowards(rb.linearVelocityX, -MaxSpeed, SmoothTime * Time.deltaTime);
            }

            rb.linearVelocityX = newSpeed;
        }
    }
}
