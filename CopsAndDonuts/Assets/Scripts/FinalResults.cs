using UnityEngine;

public class FinalResults : MonoBehaviour
{
    public GameObject greenWinPanel;
    public GameObject blueWinPanel;
    public GameObject redWinPanel;
    public GameObject tiePanel;

    public void ShowWinner()
    {
        int p1 = GameManager.instance.player1Score;
        int p2 = GameManager.instance.player2Score;
        int p3 = GameManager.instance.player3Score;

        Debug.Log("Final scores — P1: " + p1 + " P2: " + p2 + " P3: " + p3);

        if (p1 > p2 && p1 > p3)
            greenWinPanel.SetActive(true);
        else if (p2 > p1 && p2 > p3)
            blueWinPanel.SetActive(true);
        else if (p3 > p1 && p3 > p2)
            redWinPanel.SetActive(true);
        else
            tiePanel.SetActive(true);
    }
}