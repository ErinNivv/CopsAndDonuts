using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform[] donutSpots;   // Assign empty child transforms as spots
    public int winAmount = 3;
    public GameObject winPanel;

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
                donut.transform.position = donutSpots[i].position;
                donut.transform.parent = donutSpots[i];
                donut.GetComponent<Rigidbody2D>().simulated = false;

                if (CountDonuts() >= winAmount)
                    Win();

                return true; // Successfully placed
            }
        }
        return false; // Plate full
    }

    // Remove donut from plate so player can pick it up
    public void RemoveDonut(GameObject donut)
    {
        for (int i = 0; i < donutsOnPlate.Length; i++)
        {
            if (donutsOnPlate[i] == donut)
            {
                donutsOnPlate[i] = null;
                donut.transform.parent = null;
                donut.GetComponent<Rigidbody2D>().simulated = true;
                break;
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

    void Win()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log(name + " wins!");
    }
}