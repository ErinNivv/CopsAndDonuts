using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Knockback")]
    private float bounceForce = 10f;
    private float lastBounceTime;
    private float bounceCoolDown = 0.5f;
    private Vector2 bounceVelocity;
    public static PlayerControls instance;
    private bool isBouncing = false;
    private float dropDonutWaitTime = 0.3f;

    [Header("PickUp")]
    [SerializeField] Transform holdPoint;
    private GameObject heldDonut;

    [SerializeField] private Rigidbody2D rbP1;

    [Header("Door")]
    [SerializeField] private Collider2D doorCurrent;
    public GameObject openDoor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;
    public bool door1TIsOpen = false;
    public bool door1BIsOpen = false;
    public bool door2LIsOpen = false;
    public bool door2RIsOpen = false;
    public bool door3TIsOpen = false;
    public bool door3BIsOpen = false;
    private float doorOpenTime = 5f;
    //public GameObject door1T;
    //public GameObject door1B;
    //public GameObject door2L;
    //public GameObject door2R;
    //public GameObject door3T;
    //public GameObject door3B;


    [Header("Plate")]
    [SerializeField] private float plateDetect = 1.5f;
    [SerializeField] private LayerMask plateLayer;

    //private int donutsOnPlate = 0;
    //private int donutsWin = 3;
    //private bool hasWon = false;
    //input Manager
    public PlayerInput playerInput;

    [Header("Slide")]
    public float slipFriction = 0.01f;
    public float slideDecrease = 0.95f;
    public float minSlideSpeed = 0.1f;

    private bool isOnSlipperySurface;
    private bool controlDisabled;
    private float currentFriction;

 

    [Header("Animations")]
    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource playerSrc;
    [SerializeField] private AudioClip slipAudio;
    [SerializeField] private AudioClip funnyCrashAudio;
    [SerializeField] private AudioClip crashAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbP1 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        //kinda reads which player was picked 
        //CharacterData selectedCharacter = GameSessionData.Players[playerInput.playerIndex];
        //if (selectedCharacter != null && selectedCharacter.portrait != null)
        //{
        //    SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //    if (sr != null) sr.sprite = selectedCharacter.portrait;
        //}

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
        else if(!isBouncing)
        {
            rbP1.linearVelocity = new Vector2(moveP1.x * moveSpeed, moveP1.y * moveSpeed);
        }
    }

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
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

        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) return;


        if (!context.performed)
        {
            animator.SetBool("isGrabbing", true );
            animator.SetFloat("LastInputX", moveP1.x);
            animator.SetFloat("LastInputY", moveP1.y);
            StartCoroutine(StopPickUpAnim());
        }
        //if (context.canceled)
        //{
        //    animator.SetBool("isGrabbing", false);
        //}

        if (heldDonut == null)
            TryGrabDonut();
        else
            DropDonut();
    }

    IEnumerator StopPickUpAnim()
    {
        yield return new WaitForSeconds(0.20f);

        animator.SetBool("isGrabbing", false );
        animator.SetFloat("LastInputX", moveP1.x);
        animator.SetFloat("LastInputY", moveP1.y);

        yield return 0;
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
                // Let plate do the snapping to its donut spots 
                if (plate.PlaceDonut(heldDonut, this))
                {
                    heldDonut = null;
                    return;
                }
            }
        }

        // If not placed on a plate, just drop where it is
        heldDonut.transform.SetParent(null);
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
                //print("Detected Door: " + name);

                OpenedDoor1();
            }
            
            else
            {
                Debug.Log("no door in range");
            }

            Collider2D hit2 = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Door2"));
            if (hit2 != null)
            {
                OpenedDoor2();
            }
            else
            {
                Debug.Log("no door in range");
            }

            Collider2D hit3 = Physics2D.OverlapCircle(transform.position, grabRange, LayerMask.GetMask("Door3"));
            if (hit3 != null)
            {
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
        if(door1TIsOpen == true && door1BIsOpen == true)
        {
            ClosedDoor1();
        }
        else
        {
            GameObject door1T = GameObject.FindGameObjectWithTag("Door1T");
            GameObject door1B = GameObject.FindGameObjectWithTag("Door1B");

            Animator doorAnimator1T = door1T.gameObject.GetComponent<Animator>();
            Animator doorAnimator1B = door1B.gameObject.GetComponent<Animator>();

            if (doorAnimator1T != null && doorAnimator1B != null)
            {
                doorAnimator1T.SetTrigger("Open");
                doorAnimator1B.SetTrigger("Open");
                door1TIsOpen = true;
                door1BIsOpen = true;
            }
            else
            {
                Debug.Log("No animator component on door");
            }
        }
    }

    private void OpenedDoor2()
    {
        if (door2LIsOpen == true && door2RIsOpen == true)
        {
            ClosedDoor2();
        }
        else
        {
            GameObject door2L = GameObject.FindGameObjectWithTag("Door2L");
            GameObject door2R = GameObject.FindGameObjectWithTag("Door2R");

            Animator doorAnimator2L = door2L.gameObject.GetComponent<Animator>();
            Animator doorAnimator2R = door2R.gameObject.GetComponent<Animator>();

            if (doorAnimator2L != null && doorAnimator2R != null)
            {
                doorAnimator2L.SetTrigger("Open");
                doorAnimator2R.SetTrigger("Open");
                door2LIsOpen = true;
                door2RIsOpen = true;
            }
            else
            {
                Debug.Log("No animator component on door");
            }
        }
    }

    private void OpenedDoor3()
    {
        if (door3TIsOpen == true && door3BIsOpen == true)
        {
            ClosedDoor3();
        }
        else
        {
            GameObject door3T = GameObject.FindGameObjectWithTag("Door3T");
            GameObject door3B = GameObject.FindGameObjectWithTag("Door3B");

            Animator doorAnimator3T = door3T.gameObject.GetComponent<Animator>();
            Animator doorAnimator3B = door3B.gameObject.GetComponent<Animator>();

            if (doorAnimator3T != null && doorAnimator3B != null)
            {
                doorAnimator3T.SetTrigger("Open");
                doorAnimator3B.SetTrigger("Open");
                door3TIsOpen = true;
                door3BIsOpen = true;
            }
            else
            {
                Debug.Log("No animator component on door");
            }
        }
    }

    //private IEnumerator Door1()
    //{
    //    yield return new WaitForSeconds(doorOpenTime);
    //    if (door1 != null && door1IsOpen)
    //    {
    //        Animator doorAnimator = door1.gameObject.GetComponent<Animator>();
    //        if (doorAnimator != null)
    //        {
    //            doorAnimator.SetTrigger("Close");
    //        }
    //        door1IsOpen=false;
    //        Debug.Log("door closed");
    //    }
    //    yield return 0;
    //}

    private void ClosedDoor1()
    {
        if (door1TIsOpen == true && door1BIsOpen == true)
        {
            GameObject door1T = GameObject.FindGameObjectWithTag("Door1T");
            GameObject door1B = GameObject.FindGameObjectWithTag("Door1B");

            Animator doorAnimator1T = door1T.gameObject.GetComponent<Animator>();
            Animator doorAnimator1B = door1B.gameObject.GetComponent<Animator>();

            if(doorAnimator1T != null && doorAnimator1B != null)
            {
                doorAnimator1T.SetTrigger("Close");
                doorAnimator1B.SetTrigger("Close");
                Debug.Log("Trigger is set to close");
                door1TIsOpen = false;
                door1BIsOpen = false;
            }
        }
    }

    private void ClosedDoor2()
    {
        if (door2LIsOpen == true && door2RIsOpen == true)
        {
            GameObject door2L = GameObject.FindGameObjectWithTag("Door2L");
            GameObject door2R = GameObject.FindGameObjectWithTag("Door2R");

            Animator doorAnimator2L = door2L.gameObject.GetComponent<Animator>();
            Animator doorAnimator2R = door2R.gameObject.GetComponent<Animator>();

            if (doorAnimator2L != null && doorAnimator2R != null)
            {
                doorAnimator2L.SetTrigger("Close");
                doorAnimator2R.SetTrigger("Close");
                Debug.Log("Trigger is set to close");
                door2LIsOpen = false;
                door2RIsOpen = false;
            }
        }
    }

    private void ClosedDoor3()
    {
        if (door3TIsOpen == true && door3BIsOpen == true)
        {
            GameObject door3T = GameObject.FindGameObjectWithTag("Door3T");
            GameObject door3B = GameObject.FindGameObjectWithTag("Door3B");

            Animator doorAnimator3T = door3T.gameObject.GetComponent<Animator>();
            Animator doorAnimator3B = door3B.gameObject.GetComponent<Animator>();

            if (doorAnimator3T != null && doorAnimator3B != null)
            {
                doorAnimator3T.SetTrigger("Close");
                doorAnimator3B.SetTrigger("Close");
                Debug.Log("Trigger is set to close");
                door3TIsOpen = false;
                door3BIsOpen = false;
            }
        }
    }

    //private IEnumerator Door2()
    //{
    //    yield return new WaitForSeconds(doorOpenTime);
    //    if (door2 != null && door2IsOpen)
    //    {
    //        Animator doorAnimator = door2.gameObject.GetComponent<Animator>();
    //        if (doorAnimator != null)
    //        {
    //            doorAnimator.SetTrigger("Close");
    //        }
    //        door2IsOpen = false;
    //        Debug.Log("door closed");
    //    }
    //    yield return 0;
    //}

    //private IEnumerator Door3()
    //{
    //    yield return new WaitForSeconds(doorOpenTime);
    //    if (door3 != null && door3IsOpen)
    //    {
    //        Animator doorAnimator = door3.gameObject.GetComponent<Animator>();
    //        if (doorAnimator != null)
    //        {
    //            doorAnimator.SetTrigger("Close");
    //        }
    //        door3IsOpen = false;
    //        Debug.Log("door closed");
    //    }
    //    yield return 0;
    //}

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
        playerSrc.PlayOneShot(slipAudio);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            Debug.Log("Car hit!");

            Vector2 direction = (transform.position - other.transform.position).normalized;
            rbP1.AddForce(direction * bounceForce, ForceMode2D.Impulse);
            controlDisabled = true;
            playerSrc.PlayOneShot(funnyCrashAudio);
            playerSrc.PlayOneShot(crashAudio);
            StartCoroutine(HitToDropDonut());
        }
    }

    private IEnumerator HitToDropDonut()
    {
        yield return new WaitForSeconds(dropDonutWaitTime);

        if (heldDonut != null)
        {
            DropDonut();
        }

        yield return 0;
    }
}
