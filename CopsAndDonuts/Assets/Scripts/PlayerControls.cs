using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("PLAYERS")]
    private float moveSpeed = 5f;
    private float grabRange = 2.5f;

    private Vector2 moveP1;

    private float interactP1;

    [SerializeField] Transform rayP1;

    [SerializeField] private Rigidbody2D rbP1;

    [Header("Door")]
    [SerializeField] public GameObject door;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Sprites;

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
        //interactP1 = context.ReadValue<float>();
        //interactP2 = context.ReadValue<float>();
        //interactP3 = context.ReadValue<float>();

        if (context.performed)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(rayP1.position, rayP1.up, 3);
                if (hit.collider != null)
                {
                if (hit.collider.CompareTag("Donut"))
                {
                    print("Working");
                }
                }
            Debug.DrawRay(rayP1.position, rayP1.up, Color.red, 3f);
        }
        
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

    public void Door()
    {
        
    }
}
