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

    public void NextLvl2()
    {
        SceneManager.LoadScene("LEVEL 4");
    }

    public void NextLvl3()
    {
        SceneManager.LoadScene("LEVEL 1");
    }

    public void NextLvl4()
    {
        SceneManager.LoadScene("LEVEL 3");
    }

}
