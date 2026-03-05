using UnityEngine;

public class Plate : MonoBehaviour
{
    [Header("Donut Spots")]
    public Transform[] donutSpots;   // Empty child transforms as stack spots
    public int winAmount = 3;
    public GameObject winPanel;      // Assign a UI panel for win

    private GameObject[] donutsOnPlate;

    private void Awake()
    {
        donutsOnPlate = new GameObject[donutSpots.Length];
    }

    // Try to place a donut on this plate
    public bool PlaceDonut(GameObject donut)
    {
        for (int i = 0; i < donutsOnPlate.Length; i++)
        {
            if (donutsOnPlate[i] == null)
            {
                donutsOnPlate[i] = donut;

                // Snap to the spot
                donut.transform.position = donutSpots[i].position;
                donut.transform.parent = donutSpots[i]; // Parent for stacking

                if (CountDonuts() >= winAmount)
                    Win();

                return true; // Placed successfully
            }
        }
        return false; // Plate full
    }

    // Remove a donut from the plate so player can pick it up
    public void RemoveDonut(GameObject donut)
    {
        for (int i = 0; i < donutsOnPlate.Length; i++)
        {
            if (donutsOnPlate[i] == donut)
            {
                donutsOnPlate[i] = null;

                // Unparent so player can hold it
                donut.transform.parent = null;
                return;
            }
        }
    }

    public int CountDonuts()
    {
        int count = 0;
        foreach (var d in donutsOnPlate)
            if (d != null) count++;
        return count;
    }

    private void Win()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log(name + " wins!");
    }
}