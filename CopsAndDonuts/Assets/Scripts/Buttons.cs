using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour
{
    public string sceneName;
    public GameObject controlPanel;

    public GameObject levelStartPanel;
    public Slider fillBar;             
    public float panelTime = 3f;


    //void Start()
    //{
    //    // Destroy duplicate EventSystems caused by DontDestroyOnLoad objects carrying over
    //    EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
    //    if (eventSystems.Length > 1)
    //    {
    //        for (int i = 1; i < eventSystems.Length; i++)
    //        {
    //            Destroy(eventSystems[i].gameObject);
    //            Debug.Log("Destroyed duplicate EventSystem");
    //        }
    //    }
    //}
    public void OnPlayButtonPressed()
    {
        levelStartPanel.SetActive(true); 
        StartCoroutine(ShowPanelAndLoad());
    }

    private IEnumerator ShowPanelAndLoad()
    {
        fillBar.value = 0f;
        float elapsed = 0f;

        while (elapsed < panelTime)
        {
            elapsed += Time.deltaTime;
            fillBar.value = elapsed / panelTime;
            yield return null;
        }

        levelStartPanel.SetActive(false); 
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
        SceneManager.LoadScene("LEVEL 2");
        Debug.Log("Current scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log("working");
    }

    public void NextLvl3()
    {
        SceneManager.LoadScene("LEVEL 3");
    }

    public void NextLvl4()
    {
        SceneManager.LoadScene("LEVEL 4");
    }

    public void Home()
    {
        SceneManager.LoadScene("START");
    }

    public void Next()
    {
        SceneManager.LoadScene(sceneName);
    }
}
