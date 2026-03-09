using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI player3Text;
    public TextMeshProUGUI roundText;

    void Update()
    {
        player1Text.text = "P1: " + GameManager.instance.player1Score;
        player2Text.text = "P2: " + GameManager.instance.player2Score;
        player3Text.text = "P2: " + GameManager.instance.player3Score;
        roundText.text = "Round: " + GameManager.instance.currentRound + "/4";
    }
}
