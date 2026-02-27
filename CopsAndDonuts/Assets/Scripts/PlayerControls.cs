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
    private PlayerInput playerInput;

    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

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
            if (heldObject != null)
            {
                print("Dropping donut!");
                DropObject();
            }
            else
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabRange);

                foreach (var hit in hitColliders)
                {
                    if (hit.name.Contains("Donut"))
                    {
                        print("Picking up: " + hit.name);
                        PickUpObject(hit.gameObject);
                        return;
                    }
                }

                print("No donut found");
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        // Check if donut is on a plate
        Plate plate = obj.GetComponentInParent<Plate>();
        if (plate != null)
        {
            plate.RemoveDonut();
        }

        heldObject = obj;
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;

       
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        print("Donut is picked");
    }

    void DropObject()
    {
        if (heldObject == null) return;

        // Re-enable physics
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        // Re-enable collider
        Collider2D col = heldObject.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        // Check for plate
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
    }
}