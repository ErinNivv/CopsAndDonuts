using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Plate : MonoBehaviour
{
    [Header("Donut Spots")]
    public Transform[] donutSpots;
    public int winAmount = 3;
    public GameObject playerWinPanel;

    [Header("Plate Rules")]
    public bool restrictToPlayer = false;
    public int allowedPlayerID = 0;

    private GameObject[] donutsOnPlate;

    [Header("Wrong Plate Feedback")]
    public GameObject wrongPlateSprite;
    public float popupTime = 1f;

    public GameObject nextButton;
    public EventSystem eventSystem;

    private bool roundFinished = false;

    private void Awake()
    {
        donutsOnPlate = new GameObject[donutSpots.Length];
    }

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

                Vector3 originalScale = donut.transform.lossyScale;

                donut.transform.parent = null;
                donut.transform.position = donutSpots[i].position;
                donut.transform.SetParent(donutSpots[i]);

                donut.transform.localPosition = Vector3.zero;
                donut.transform.localRotation = Quaternion.identity;

                donut.transform.localScale = new Vector3(
                    originalScale.x / donutSpots[i].lossyScale.x,
                    originalScale.y / donutSpots[i].lossyScale.y,
                    originalScale.z / donutSpots[i].lossyScale.z
                );

                SpriteRenderer sr = donut.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sortingOrder = 5;

                if (CountDonuts() >= winAmount)
                    Win(player);

                return true;
            }
        }

        return false;
    }

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
            if (d != null) count++;
        }
        return count;
    }

    void Win(PlayerControls player)
    {
        if (roundFinished) return;
        roundFinished = true;

        int playerIndex = player.playerInput.playerIndex;
        Debug.Log("Plate won by player " + playerIndex);

        // Tell GameManager to update score
        if (GameManager.instance != null)
            GameManager.instance.PlayerWon(playerIndex);

        // Show this plate's win panel
        if (playerWinPanel != null)
            playerWinPanel.SetActive(true);

        if (nextButton != null)
        {
            nextButton.SetActive(true);
            StartCoroutine(SelectNextButton());
        }
    }

    IEnumerator SelectNextButton()
    {
        yield return new WaitForSeconds(0.5f);
        if (eventSystem != null)
            eventSystem.SetSelectedGameObject(nextButton);
    }

    IEnumerator ShowWrongPlate()
    {
        wrongPlateSprite.SetActive(true);
        yield return new WaitForSeconds(popupTime);
        wrongPlateSprite.SetActive(false);
    }
}