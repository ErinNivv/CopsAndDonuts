using UnityEngine;

public class FinalResults : MonoBehaviour
{
    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject player3Panel;
    public GameObject tiePanel;

    void Start()
    {
        int p1 = GameManager.instance.player1Score;
        int p2 = GameManager.instance.player2Score;
        int p3 = GameManager.instance.player3Score;

        if (p1 > p2 && p1 > p3)
        {
            player1Panel.SetActive(true);
        }
        else if (p2 > p1 && p2 > p3)
        {
            player2Panel.SetActive(true);
        }
        else if (p3 > p1 && p3 > p2)
        {
            player3Panel.SetActive(true);
        }
        else
        {
            tiePanel.SetActive(true);
        }
    }
}