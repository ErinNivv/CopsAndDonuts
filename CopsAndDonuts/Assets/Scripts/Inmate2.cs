using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Inmate2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Transform target;
    Vector2 movedirection;
    [SerializeField] private BoxCollider2D bc;

    private Animator inmateAnimator;
    private bool isAtTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        inmateAnimator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject found = GameObject.FindGameObjectWithTag("Target2");
        target = found != null ? found.transform : null;

        if (target && !isAtTarget)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movedirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (isAtTarget) return;

        if (target)
        {
            rb.linearVelocity = new Vector2(movedirection.x, movedirection.y) * moveSpeed;
            inmateAnimator.SetBool("isWalking", true);

            inmateAnimator.SetFloat("X Axis", movedirection.x);
            inmateAnimator.SetFloat("Y Axis", movedirection.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            inmateAnimator.SetBool("isWalking", false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target2") && !isAtTarget)
        {
            isAtTarget = true;
            StartCoroutine(WaitAtTarget());
        }
    }

    IEnumerator WaitAtTarget()
    {
        rb.linearVelocity = Vector2.zero;
        inmateAnimator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2f);
        isAtTarget = false;
    }
}
