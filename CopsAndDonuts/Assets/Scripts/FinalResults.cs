using UnityEngine;

public class FinalResults : MonoBehaviour
{
    public GameObject greenWinPanel;
    public GameObject blueWinPanel;
    public GameObject redWinPanel;
    public GameObject tiePanel;

    public AudioSource sceneSrc;
    public AudioClip winSfx;
    //public AudioClip tieSfx;

    public void ShowWinner()
    {
        int p1 = GameManager.instance.player1Score;
        int p2 = GameManager.instance.player2Score;
        int p3 = GameManager.instance.player3Score;

        Debug.Log("Final scores — P1: " + p1 + " P2: " + p2 + " P3: " + p3);

        if (p1 > p2 && p1 > p3)
        {
            greenWinPanel.SetActive(true);
            sceneSrc.PlayOneShot(winSfx);
        }
            
        else if (p2 > p1 && p2 > p3)
        {
            blueWinPanel.SetActive(true);
            sceneSrc.PlayOneShot(winSfx);
        }
            
        else if (p3 > p1 && p3 > p2)
        {
            redWinPanel.SetActive(true);
            sceneSrc.PlayOneShot(winSfx);
        }

        else
        {
            //tiePanel.SetActive(true);
            //sceneSrc.PlayOneShot(tieSfx);
        }
    }
}