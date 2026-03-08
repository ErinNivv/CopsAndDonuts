using UnityEngine;
using System.Collections;

public class Plate : MonoBehaviour
{
    [Header("Donut Spots")]
    public Transform[] donutSpots;   // Empty child transforms as stack spots
    public int winAmount = 3;
    public GameObject player1WinPanel;
    public GameObject player2WinPanel;
    public GameObject player3WinPanel;

    

    [Header("Plate Rules")]
    public bool restrictToPlayer = false;   // Turn ON for Level 1
    public int allowedPlayerID = 0;         // 0 = Red Player, 1 = Blue Player

    private GameObject[] donutsOnPlate;

    [Header("Wrong Plate Feedback")]
    public GameObject wrongPlateSprite;   // assign sprite object in inspector
    public float popupTime = 1f;

    private void Awake()
    {
        donutsOnPlate = new GameObject[donutSpots.Length];
    }

    // Try to place a donut on this plate
    public bool PlaceDonut(GameObject donut, PlayerControls player)
    {
        if (restrictToPlayer && player.playerInput.playerIndex != allowedPlayerID)
        {
            if (wrongPlateSprite != null)
                StartCoroutine(ShowWrongPlate());
            return false;
        }

        for (int i = 0; i < donutsOnPlate.Length; i++)
        {
            if (donutsOnPlate[i] == null)
            {
                donutsOnPlate[i] = donut;

                // Store world scale
                Vector3 originalScale = donut.transform.lossyScale;

                // Snap to spot
                donut.transform.parent = null;
                donut.transform.position = donutSpots[i].position;
                donut.transform.SetParent(donutSpots[i]);

                donut.transform.localPosition = Vector3.zero;
                donut.transform.localRotation = Quaternion.identity;

                // Fix scale
                donut.transform.localScale = new Vector3(
                    originalScale.x / donutSpots[i].lossyScale.x,
                    originalScale.y / donutSpots[i].lossyScale.y,
                    originalScale.z / donutSpots[i].lossyScale.z
                );

                // Sorting order above plate
                SpriteRenderer sr = donut.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sortingOrder = 5;

                // Check win
                if (CountDonuts() >= winAmount)
                    Win(player);

                return true;
            }
        }

        return false;
    }

    // Remove a donut from the plate so a player can pick it up
    public void RemoveDonut(GameObject donut)
    {
        for (int i = 0; i < donutsOnPlate.Length; i++)
        {
            if (donutsOnPlate[i] == donut)
            {
                donutsOnPlate[i] = null;

                donut.transform.parent = null;
                return;
            }
        }
    }

    public int CountDonuts()
    {
        int count = 0;

        foreach (var d in donutsOnPlate)
        {
            if (d != null)
                count++;
        }

        return count;
    }

    void Win(PlayerControls player)
    {
        int playerIndex = player.playerInput.playerIndex;

        if (playerIndex == 0 && player1WinPanel != null)
        {
            player1WinPanel.SetActive(true);
        }
        else if (playerIndex == 1 && player2WinPanel != null)
        {
            player2WinPanel.SetActive(true);
        }
        else if (playerIndex == 2 && player3WinPanel != null)
        {
            player3WinPanel.SetActive(true);
        }

        Debug.Log("Player " + playerIndex + " wins!");
    }

    IEnumerator ShowWrongPlate()
    {
        wrongPlateSprite.SetActive(true);

        yield return new WaitForSeconds(popupTime);

        wrongPlateSprite.SetActive(false);
    }

}