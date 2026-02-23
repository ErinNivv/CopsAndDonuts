using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERS")]
    private float moveSpeed = 5f;
    private float grabRange = 2.5f;

    private Vector2 moveP1;
    private Vector2 moveP2;
    private Vector2 moveP3;

    private float interactP1;
    private float interactP2;
    private float interactP3;

    [SerializeField] Transform rayP1;

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

        //if (interactP1 > 1)
        //{
        //    Ray ray = new Ray (rayP1.position,rayP1.forward);
        //    RaycastHit2D hit;

        //    Debug.DrawRay(rayP1.position, rayP1.forward * grabRange, Color.red, 2f);

        //    if (Physics.Raycast(ray, out hit, grabRange))
        //    {
        //        if (hit.collider.CompareTag("Donut"))
        //        {
        //            print("picked up");
        //        }
        //    }
        //}

        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveP1 = context.ReadValue<Vector2>();
        moveP2 = context.ReadValue<Vector2>();
        moveP3 = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        interactP1 = context.ReadValue<float>();
        interactP2 = context.ReadValue<float>();
        interactP3 = context.ReadValue<float>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interactP1 > 0)
        {
            if (other.CompareTag("Donut"))
            {
                print("pick up");
            }
        }
            
    }
}
