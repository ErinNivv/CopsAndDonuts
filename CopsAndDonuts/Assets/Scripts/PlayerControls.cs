using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERS")]
    private float moveSpeed = 5f;
    private float grabRange = 1.5f;
    private Vector2 moveP1;
    [SerializeField] private Rigidbody2D rbP1;

    private float interactP1;

    [SerializeField] Transform rayP1;

    [Header("PickUp")]
    [SerializeField] Transform holdPoint;
    private GameObject heldDonut;


    [Header("Door")]
    [SerializeField] public GameObject door;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;

    //[Header("Plate")]
    //[SerializeField] private float plateDetect = 1.5f;
    //[SerializeField] private LayerMask plateLayer;

    //private int donutsOnPlate = 0;
    //private int donutsWin = 3;
    //private bool hasWon = false;
    ////input Manager
    private PlayerInput playerInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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


    void Update()
    {
        
        rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);

    }

    //void FixedUpdate()
    //{
    //    if (heldObject != null)
    //    {
    //        // Snap donut directly to holdPoint
    //        heldObject.transform.position = holdPoint.position;
    //        heldObject.transform.rotation = holdPoint.rotation;
    //    }
    //}

    public void Move(InputAction.CallbackContext context)
    {
        moveP1 = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("INTERACT BUTTON PRESSED!");

        if (!context.performed) return;
        {
            Debug.Log("Context is performed!");

            if (heldDonut == null)
            {
                Debug.Log("Dropping donut!");
                TryGrabDonut();
            }
            else
            {
                Debug.Log("Trying to pickup...");
                DropDonut();
            }
        }
    }

    void TryGrabDonut()
    {
        // Detect all donuts in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Donut"));
        if (hits.Length == 0) return;

        // Pick the closest one
        Collider2D closest = hits[0];
        float closestDist = Vector2.Distance(transform.position, closest.transform.position);

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closest = hit;
                closestDist = dist;
            }
        }

        heldDonut = closest.gameObject;

        // If it was on a plate, remove it
        Plate plate = heldDonut.transform.parent?.GetComponentInParent<Plate>();
        if (plate != null)
        {
            plate.RemoveDonut(heldDonut);
        }

        // Grab it
        heldDonut.transform.parent = holdPoint;
        heldDonut.transform.position = holdPoint.position;
        heldDonut.GetComponent<Rigidbody2D>().simulated = false;
    }



    void DropDonut()
    {  // Check for nearby plate
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Plate"));
        if (hit != null)
        {
            Plate plate = hit.GetComponent<Plate>();
            if (plate != null && plate.PlaceDonut(heldDonut))
            {
                heldDonut = null; // Successfully placed
                return;
            }
        }

        // If no plate or plate full, drop in world
        heldDonut.GetComponent<Rigidbody2D>().simulated = true;
        heldDonut.transform.parent = null;
        heldDonut = null;
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
                door = hit.collider.gameObject;
                door.gameObject.SetActive(false);
                StartCoroutine(Door());

                print("Working Door: " + hit.collider.name);
            }
        }
    }

    IEnumerator Door()
    {
        yield return new WaitForSeconds(7f);
        door.gameObject.SetActive(true);

        yield return null;
    }
}