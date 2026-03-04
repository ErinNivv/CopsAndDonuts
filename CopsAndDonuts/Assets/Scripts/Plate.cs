using UnityEngine;

public class Plate : MonoBehaviour
{
    [Header("Settings")]
    public Transform[] donutPoints;
    public GameObject winPanel;

    public void PlaceDonut(GameObject donut)
    {
        for (int i = 0; i < donutPoints.Length; i++)
        {
            if (donutPoints[i].childCount == 0)
            {
                donut.transform.position = donutPoints[i].position;
                donut.transform.rotation = donutPoints[i].rotation;
                donut.transform.SetParent(donutPoints[i]);

                // Disable physics ONLY - DO NOT disable collider!
                Rigidbody2D rb = donut.GetComponent<Rigidbody2D>();
                if (rb != null) rb.simulated = false;

                Debug.Log("Donut placed on plate! Collider enabled: " +
                    (donut.GetComponent<Collider2D>() != null ?
                    donut.GetComponent<Collider2D>().enabled : "no collider"));

                CheckWin();
                return;
            }
        }
    }

    public void RemoveDonut(GameObject donut)
    {
        // Re-enable physics so it can be picked up
        Rigidbody2D rb = donut.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        // Remove from plate point
        donut.transform.SetParent(null);

        Debug.Log("Donut removed from plate");
    }

    void CheckWin()
    {
        int donutCount = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Donut")) donutCount++;
        }

        if (donutCount >= 3)
        {
            Debug.Log("YOU WIN!");
            if (winPanel != null) winPanel.SetActive(true);
        }
    }
}