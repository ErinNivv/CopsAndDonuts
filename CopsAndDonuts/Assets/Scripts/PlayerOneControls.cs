using UnityEngine;
using UnityEngine.InputSystem;

public class PayerOneControls : MonoBehaviour
{
    [Header("PLAYER3")]
    private float moveSpeed = 5f;
    private float grabRange = 2.5f;

    private Vector2 moveP1;

    private float interactP1;

    [SerializeField] Transform rayP1;

    [SerializeField] private Rigidbody2D rbP1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);

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
    }

    public void Interact(InputAction.CallbackContext context)
    {
        interactP1 = context.ReadValue<float>();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (interactP1 > 0)
    //    {
    //        if (other.CompareTag("Donut"))
    //        {
    //            print("pick up");
    //        }
    //    }

    //}
}
