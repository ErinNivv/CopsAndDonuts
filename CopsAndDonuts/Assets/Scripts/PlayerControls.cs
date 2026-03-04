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

    private float interactP1;

    [SerializeField] Transform rayP1;

    [Header("PickUp")]
    [SerializeField] Transform rayPoint;
    [SerializeField] Transform holdPoint;
    private GameObject heldObject;

    [SerializeField] private Rigidbody2D rbP1;

    [Header("Door")]
    [SerializeField] public GameObject door;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;

    [Header("Plate")]
    [SerializeField] private float plateDetect = 1.5f;
    [SerializeField] private LayerMask plateLayer;

    private int donutsOnPlate = 0;
    private int donutsWin = 3;
    private bool hasWon = false;
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
        if (context.performed)
        {
            if (heldObject != null)
            {
                print("Dropping donut!");
                DropObject();
            }
            else
            {
                PickupObject();
            }
        }
    }

    void PickupObject()
    {
        Debug.Log("=== PICKUP ATTEMPT ===");
        Debug.Log("Player Position: " + transform.position);
        Debug.Log("Grab Range: " + grabRange);

        // Check ALL around player
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabRange);

        Debug.Log("Found " + hitColliders.Length + " objects nearby");

        foreach (var hit in hitColliders)
        {
            Debug.Log("Object: " + hit.name + " | Tag: " + hit.tag + " | Layer: " + hit.gameObject.layer);

            if (hit.CompareTag("Donut"))
            {
                Debug.Log("FOUND DONUT! Picking up: " + hit.name);
                PickUpObject(hit.gameObject);
                return;
            }
        }

        Debug.Log("No donut found to pick up");
    }

    void PickUpObject(GameObject obj)
    {
        // Remove from plate if necessary
        Plate plate = obj.GetComponentInParent<Plate>();
        if (plate != null)
        {
            plate.RemoveDonut(obj);
        }

        heldObject = obj;

        // Parent to holdPoint
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;

        // Disable physics only
        Rigidbody2D rbObj = obj.GetComponent<Rigidbody2D>();
        if (rbObj != null) rbObj.simulated = false;
    }

    void DropObject()
    {
        if (heldObject == null) return;

        // Re-enable physics
        Rigidbody2D rbObj = heldObject.GetComponent<Rigidbody2D>();
        if (rbObj != null) rbObj.simulated = true;

        // Check plate
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, plateDetect, plateLayer);

        foreach (var hit in hitColliders)
        {
            Plate plate = hit.GetComponent<Plate>();
            if (plate != null)
            {
                plate.PlaceDonut(heldObject);
                heldObject = null;
                return;
            }
        }

        // Drop normally
        heldObject.transform.SetParent(null);
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