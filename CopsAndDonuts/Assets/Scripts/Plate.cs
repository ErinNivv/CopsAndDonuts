using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Plate : MonoBehaviour
{
    [Header("Donut Spots")]
    public Transform[] donutSpots;
    public int winAmount = 3;

    [Header("This Plate's Win Panel")]
    public GameObject thisPlateWinPanel;
    public GameObject nextButton;

    [Header("Wrong Plate Feedback")]
    public GameObject wrongPlateSprite;
    public float popupTime = 1f;

    public EventSystem eventSystem;

    private GameObject[] donutsOnPlate;
    private bool roundFinished = false;

    private void Awake()
    {
        donutsOnPlate = new GameObject[donutSpots.Length];
    }

    public bool PlaceDonut(GameObject donut, PlayerControls player)
    {
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
                    Win();

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

    void Win()
    {
        if (roundFinished) return;
        roundFinished = true;

        // Just show this plate's own win panel — nothing else
        if (thisPlateWinPanel != null)
            thisPlateWinPanel.SetActive(true);

        if (nextButton != null)
            nextButton.SetActive(true);
    }

    IEnumerator ShowWrongPlate()
    {
        wrongPlateSprite.SetActive(true);
        yield return new WaitForSeconds(popupTime);
        wrongPlateSprite.SetActive(false);
    }
}