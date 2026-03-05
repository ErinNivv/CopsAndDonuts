using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    public float friction = 0.1f;
    public float slideDecrease = 0.95f;
    public float minSlideSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControls player = other.gameObject.GetComponent<PlayerControls>();
            if (player != null)
            {
                player.OnEnterSlipperySurface(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControls player = other.GetComponent<PlayerControls>();
            if(player != null)
            {
                player.OnExitSlipperySurface();
            }
        }
    }
}
