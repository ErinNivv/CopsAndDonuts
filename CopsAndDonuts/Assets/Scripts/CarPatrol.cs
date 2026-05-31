using System.Runtime.CompilerServices;
using UnityEngine;

public class CarPatrol : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Transform currentPoint;

    [Header("Car")]
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    [SerializeField] private float carSpeed;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(carSpeed, 0);
            sr.flipX = true;
        }
        else
        {
            rb.linearVelocity = new Vector2 (-carSpeed, 0);
            sr.flipX = false;
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;

        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Barrier") || other.gameObject.CompareTag("Donut"))
        {
            bc = GetComponent<BoxCollider2D>();
            bc.isTrigger = true;
            Debug.Log("Car is entered barrier");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrier") || other.gameObject.CompareTag("Donut"))
        {
            bc = GetComponent<BoxCollider2D>();
            bc.isTrigger = false;
            Debug.Log("Car is exited barrier");
        }
    }
}
