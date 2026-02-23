using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERs")]
    private float moveSpeed = 5f;

    private Vector2 moveP1;
    private Vector2 moveP2;
    private Vector2 moveP3;

    [SerializeField] private Rigidbody2D rbP1;
    [SerializeField] private Rigidbody2D rbP2;
    [SerializeField] private Rigidbody2D rbP3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
        rbP2 = GetComponent<Rigidbody2D>();
        rbP3 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);
        rbP2.linearVelocity = new Vector2(moveP2.x * moveSpeed, moveP2.y * moveSpeed);
        rbP3.linearVelocity = new Vector2(moveP3.x * moveSpeed, moveP3.y * moveSpeed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveP1 = context.ReadValue<Vector2>();
        moveP2 = context.ReadValue<Vector2>();
        moveP3 = context.ReadValue<Vector2>();
    }
}
