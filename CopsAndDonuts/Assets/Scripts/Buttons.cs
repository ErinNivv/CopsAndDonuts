using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public string sceneName;
    public GameObject controlPanel;
    public void PlayButton()
    {
        SceneManager.LoadScene("LEVEL 2");
    }

    public void ControlPanel()
    {
        controlPanel.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("START");
    }

    public void BackButton()
    {
        controlPanel.SetActive(false);
    }



}
