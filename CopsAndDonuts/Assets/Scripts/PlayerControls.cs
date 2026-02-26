using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERS")]
    private float moveSpeed = 5f;
    private float grabRange = 1f;

    private Vector2 moveP1;

    private float interactP1;

    [SerializeField] Transform rayP1;

    [Header("PickUp")]
    [SerializeField] Transform holdPoint;
    private GameObject heldObject;

    [SerializeField] private Rigidbody2D rbP1;

    [Header("Door")]
    [SerializeField] private GameObject doorCurrent;
    public GameObject openDoor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;
    public bool doorIsOpen = false;
    private float doorOpenTime = 5f;
    


    //input Manager
    private PlayerInput playerInput;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // if its Player 1
        if (playerInput.playerIndex == 0)
        {
            //Assign Sprites

        }
        else if (playerInput.playerIndex == 1)//if its Player 2
        {
            //Assign Sprites
        }

    }

    // Update is called once per frame
    void Update()
    {
        rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);


    }

    public void Move(InputAction.CallbackContext context)
    {
        moveP1 = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Check if already holding something
            if (heldObject != null)
            {
                // DROP the donut
                print("Dropping donut!");
                DropObject();
            }
            else
            {
                // PICK UP a donut
                Vector2 direction = transform.right;
                int layerMask = LayerMask.GetMask("Donut");

                RaycastHit2D hit = Physics2D.Raycast(rayP1.position, direction, grabRange, layerMask);
                Debug.DrawRay(rayP1.position, direction * grabRange, Color.red, 0.5f);

                if (hit.collider != null)
                {
                    print("Working Donut: " + hit.collider.name);
                    PickUpObject(hit.collider.gameObject);
                }
                else
                {
                    print("No donut found to pick up");
                }
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;

        // Make it a child of the hold point
        obj.transform.SetParent(holdPoint);

        // Reset position to match hold point
        obj.transform.localPosition = Vector3.zero;

        // Disable physics so it doesn't interfere while held
        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.simulated = false;
        }
        if (obj.TryGetComponent<Collider2D>(out Collider2D col))
        {
            col.enabled = false;
        }

        print("Donut is picked");
    }

    void DropObject()
    {

        if (heldObject == null) return;

        // Turm the physics back on 
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }

        Collider2D col = heldObject.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }

        heldObject.transform.SetParent(null);
        print("Donut is Dropped");
        heldObject = null;

    }

    public void OpenDoor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 direction = transform.right;
            int layerMask = LayerMask.GetMask("Door");

            RaycastHit2D hit = Physics2D.Raycast(rayP1.position, direction, grabRange, layerMask);
            Debug.DrawRay(rayP1.position, direction * grabRange, Color.red, 0.5f);

            if (hit.collider != null)
            {
                GameObject door = hit.collider.gameObject;
                print("Detected Door: " + hit.collider.name);

                OpenedDoor(door);
            }
            else
            {
                Debug.Log("no door in range");
            }
        }
    }

    private void OpenedDoor(GameObject door)
    {
        if(doorIsOpen && doorCurrent == door)
            return;
        doorCurrent = door;
        door.SetActive(false);
        doorIsOpen = true;
        StartCoroutine(Door());
    }

    private IEnumerator Door()
    {
        yield return new WaitForSeconds(doorOpenTime);
        if (doorCurrent != null && doorIsOpen)
        {
            doorCurrent.SetActive(true);
            doorIsOpen=false;
            Debug.Log("door closed");
        }
    }
}
