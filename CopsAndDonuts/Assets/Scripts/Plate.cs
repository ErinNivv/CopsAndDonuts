using UnityEngine;

public class Plate : MonoBehaviour
{
    public int donutCount = 0;
    public int donutsToWin = 3;

    [Header("Settings")]
    public Transform[] donutPoints;
    public GameObject winPanel;

    public void PlaceDonut(GameObject donut)
    {
        if (donutCount >= donutsToWin || donutCount >= donutPoints.Length) return;

        Transform targetPoint = donutPoints[donutCount];

        donut.transform.position = targetPoint.position;
        donut.transform.rotation = targetPoint.rotation;
        donut.transform.SetParent(targetPoint);

        Rigidbody2D rb = donut.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        donutCount++;
    }

    public void RemoveDonut()
    {
        if (donutCount > 0)
        {
            donutCount--;
            Debug.Log("Donut removed! Total: " + donutCount);
        }
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
}