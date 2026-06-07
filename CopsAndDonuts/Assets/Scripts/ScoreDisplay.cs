using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI player3Text;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        player1Text.text = "" + GameManager.instance.player1Score;
        player2Text.text = "" + GameManager.instance.player2Score;
        player3Text.text = "" + GameManager.instance.player3Score; // was showing P2 score before
    }
}