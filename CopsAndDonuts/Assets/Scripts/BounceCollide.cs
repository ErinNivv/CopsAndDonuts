using UnityEngine;

public class BounceCollide : MonoBehaviour
{
    public float bounceForce = 100f;
    public float bounceTime = 1;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayerControls.instance.Bounce(bounceTime, bounceForce, this.transform));
        }
    }
}
