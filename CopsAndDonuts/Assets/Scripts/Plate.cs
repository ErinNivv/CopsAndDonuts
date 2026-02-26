using UnityEngine;

public class Plate : MonoBehaviour
{
    public int donutCount = 0;
    public int donutsToWin = 3;
    public Transform donutPlate;

    [Header("UI")]
    public GameObject winPanel;

    // This function is called by the Player
    public void PlaceDonut(GameObject donut)
    {
        if (donutCount >= donutsToWin) return;

        // Snap donut to plate point
        donut.transform.position = donutPlate.position;
        donut.transform.rotation = donutPlate.rotation;
        donut.transform.SetParent(donutPlate);

        // Disable physics
        Rigidbody2D rb = donut.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        donutCount++;
        Debug.Log("Donut placed! Total: " + donutCount);

        if (donutCount >= donutsToWin)
        {
            WinGame();
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

