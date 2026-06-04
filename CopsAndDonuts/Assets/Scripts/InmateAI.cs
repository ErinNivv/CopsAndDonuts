using UnityEngine;

public class InmateAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Transform target;
    Vector2 movedirection;
    [SerializeField] private BoxCollider2D bc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject found = GameObject.FindGameObjectWithTag("Target");
        target = found != null ? found.transform : null;

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movedirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            rb.linearVelocity = new Vector2(movedirection.x, movedirection.y) * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Barrier") || other.gameObject.CompareTag("Donut"))
        {
            bc.GetComponent<BoxCollider2D>();
            bc.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrier") || other.gameObject.CompareTag("Donut"))
        {
            bc.GetComponent<BoxCollider2D>();
            bc.isTrigger = false;
        }
    }
}
