using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERS")]
    private float moveSpeed = 5f;
    private float grabRange = 1f;
   
    public int playerID;

    private Vector2 moveP1;

    private float interactP1;

    [SerializeField] Transform rayP1;
    private GameObject player;
    private bool isFacingRight;

    [Header("BOUNCE")]
    private float bounceForce = 10f;
    private float lastBounceTime;
    private float bounceCoolDown = 0.5f;
    private Vector2 bounceVelocity;
    public static PlayerControls instance;


    [Header("PickUp")]
    [SerializeField] Transform holdPoint;
    private GameObject heldDonut;

    [SerializeField] private Rigidbody2D rbP1;

    [Header("Door")]
    [SerializeField] private Collider2D doorCurrent;
    public GameObject openDoor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;
    public bool door1IsOpen = false;
    public bool door2IsOpen = false;
    public bool door3IsOpen = false;
    private float doorOpenTime = 5f;
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;


    [Header("Plate")]
    [SerializeField] private float plateDetect = 1.5f;
    [SerializeField] private LayerMask plateLayer;

    private int donutsOnPlate = 0;
    private int donutsWin = 3;
    private bool hasWon = false;
    //input Manager
    public PlayerInput playerInput;

    [Header("Slide")]
    public float slipFriction = 0.01f;
    public float slideDecrease = 0.95f;
    public float minSlideSpeed = 0.1f;

    private bool isOnSlipperySurface;
    private bool controlDisabled;
    private float currentFriction;

    [Header("Push")]
    private float pushRange = 0.5f;
    private float pushBack = 25f;

    [Header("Animations")]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

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
    void FixedUpdate()
    {
        if (controlDisabled)
        {
            rbP1.linearVelocity *= slideDecrease;

            if(rbP1.linearVelocity.magnitude < minSlideSpeed)
            {
                controlDisabled = false;
                Debug.Log("Controls re-enabled");
            }
        }
        else
        {
            rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);
        }

    }

    public void Awake()
    {
        instance = this;

        currentFriction = 0.1f;
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking",false);
        }

        moveP1 = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveP1.x);
        animator.SetFloat("InputY", moveP1.y);
        if (moveP1.y < 0)
        {

        }
        else if (moveP1.y > 0)
        {

        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldDonut == null)
            TryGrabDonut();
        else
            DropDonut();
    }

    void TryGrabDonut()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, grabRange, LayerMask.GetMask("Donut"));

        if (hits.Length == 0) return;

        // Get closest donut
        Collider2D closest = hits[0];
        float closestDist = Vector2.Distance(transform.position, closest.transform.position);

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closest = hit;
                closestDist = dist;
            }
        }

        heldDonut = closest.gameObject;

        // If donut was on a plate, remove it
        Plate plate = heldDonut.GetComponentInParent<Plate>();
        if (plate != null)
        {
            plate.RemoveDonut(heldDonut);
        }

        PickUpObject(heldDonut);
    }


    void PickUpObject(GameObject obj)
    {
        heldDonut = obj;

        // Store world scale before parenting
        Vector3 originalScale = obj.transform.lossyScale;

        // Parent to hold point
        obj.transform.SetParent(holdPoint);

        // Snap to hold point
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Force original world scale (prevents weird scaling)
        obj.transform.localScale = Vector3.one;
        obj.transform.localScale = new Vector3(originalScale.x / holdPoint.lossyScale.x,originalScale.y / holdPoint.lossyScale.y,originalScale.z / holdPoint.lossyScale.z);

        // Make sure donut renders above player
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 10;
    }

    void DropDonut()
    {
        // Check if player is over a plate
        Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Plate"));

        if (hit != null)
        {
            Plate plate = hit.GetComponent<Plate>();
            if (plate != null)
            {
                // Let plate handle snapping to its donut spots
                if (plate.PlaceDonut(heldDonut, this))
                {
                    heldDonut = null;
                    return;
                }
            }
        }

        // If not placed on a plate, just drop at current position
        heldDonut.transform.parent = null;
        heldDonut.transform.localPosition = Vector3.zero; // optional snap
        heldDonut = null;
    }

    public void OpenDoor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Vector2 direction = transform.right;
            //int layerMask = LayerMask.GetMask("Door");

            //RaycastHit2D hit = Physics2D.Raycast(rayP1.position, direction, grabRange, layerMask);
            //Debug.DrawRay(rayP1.position, direction * grabRange, Color.red, 0.5f);

            Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Door"));

            if (hit != null)
            {
                door1 = hit.gameObject;
                print("Detected Door: " + door1.name);

                OpenedDoor1();
            }
            
            else
            {
                Debug.Log("no door in range");
            }

            Collider2D hit2 = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Door2"));
            if (hit2 != null)
            {
                door2 = hit2.gameObject;
                print("Detected Door: " + door2.name);

                OpenedDoor2();
            }
            else
            {
                Debug.Log("no door in range");
            }

            Collider2D hit3 = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Door3"));
            if (hit3 != null)
            {
                door3 = hit3.gameObject;
                print("Detected Door: " + door3.name);

                OpenedDoor3();
            }
            else
            {
                Debug.Log("no door in range");
            }

        }
    }

    private void OpenedDoor1()
    {
        if(door1IsOpen)
            return;
        door1IsOpen = true;

        Animator doorAnimator = door1.gameObject.GetComponent<Animator>();

        if(doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("No animator component on door");
        }
        StartCoroutine(Door1());
    }

    private void OpenedDoor2()
    {
        if (door2IsOpen)
            return;
        door2IsOpen = true;

        Animator doorAnimator = door2.gameObject.GetComponent<Animator>();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("No animator component on door");
        }
        StartCoroutine(Door2());
    }

    private void OpenedDoor3()
    {
        if (door3IsOpen)
            return;
        door3IsOpen = true;

        Animator doorAnimator = door3.gameObject.GetComponent<Animator>();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("No animator component on door");
        }
        StartCoroutine(Door3());
    }

    private IEnumerator Door1()
    {
        yield return new WaitForSeconds(doorOpenTime);
        if (door1 != null && door1IsOpen)
        {
            Animator doorAnimator = door1.gameObject.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Close");
            }
            door1IsOpen=false;
            Debug.Log("door closed");
        }
        yield return 0;
    }

    private IEnumerator Door2()
    {
        yield return new WaitForSeconds(doorOpenTime);
        if (door2 != null && door2IsOpen)
        {
            Animator doorAnimator = door2.gameObject.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Close");
            }
            door2IsOpen = false;
            Debug.Log("door closed");
        }
        yield return 0;
    }

    private IEnumerator Door3()
    {
        yield return new WaitForSeconds(doorOpenTime);
        if (door3 != null && door3IsOpen)
        {
            Animator doorAnimator = door3.gameObject.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Close");
            }
            door3IsOpen = false;
            Debug.Log("door closed");
        }
        yield return 0;
    }

    //public void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("PLAYER"))
    //    {
    //        StartCoroutine(PlayerControls.instance.Bounce(lastBounceTime, bounceCoolDown, this.transform));
    //    }
    //}

    //public IEnumerator Bounce(float bounceTime, float bounceForce, Transform obj)
    //{
    //    float timer = 0;
    //    while(bounceTime > timer)
    //    {
    //        timer += Time.deltaTime;
    //        Vector2 direction = (this.transform.position - obj.transform.position).normalized;
    //        rbP1.AddForce(direction * bounceForce);
    //    }

    //    yield return 0;
    //}

    public void OnEnterSlipperySurface(SlipperySurface surface)
    {
        animator.SetBool("isSliding", true);
        animator.SetFloat("LastInputX", moveP1.x);
        animator.SetFloat("LastInputY", moveP1.y);
        isOnSlipperySurface = true;
        controlDisabled = true;
        currentFriction = slipFriction;
        Debug.Log("Controls disabled and sliding");
    }

    public void OnExitSlipperySurface()
    {
        animator.SetBool("isSliding", false);
        isOnSlipperySurface = false;
        controlDisabled = false;
        currentFriction = 0.1f;
        Debug.Log("Controls enabled");
    }

    //public void Push(InputAction.CallbackContext context)
    //{
    //    if(context.performed)
    //    {
    //        Vector2 direction = transform.right;
    //        int layerMask = LayerMask.GetMask("Player");

    //        RaycastHit2D hit = Physics2D.Raycast(rayP1.position, direction, pushRange, layerMask);
    //        Debug.DrawRay(rayP1.position, direction * pushRange, Color.red, 0.5f);

    //        if(hit.collider != null)
    //        {
    //            Rigidbody2D otherRB = hit.collider.gameObject.GetComponent<Rigidbody2D>();
    //            Debug.Log("collider hit");

    //            if(otherRB != null)
    //            {
    //                Vector2 directionToTarget = (transform.position - hit.collider.gameObject.transform.position).normalized;
    //                otherRB.AddForce(directionToTarget * pushBack, ForceMode2D.Impulse);
    //                Debug.Log("Player pushed");
    //            }
    //            else
    //            {
    //                Debug.Log("No Rigidbody");
    //            }
              
    //        }
    //        else
    //        {
    //            Debug.Log("No player in range");
    //        }

    //    }
    //}

    public void flipSprite()
    {

    }
}
