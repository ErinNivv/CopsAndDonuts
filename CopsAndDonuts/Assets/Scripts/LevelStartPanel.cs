using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelStartPanel : MonoBehaviour
{
    public GameObject panel;
    public Slider fillBar;
    public float displayTime = 3f; // seconds

    public void ShowPanel()
    {
        StartCoroutine(ShowPanelRoutine());
    }

    private IEnumerator ShowPanelRoutine()
    {
        panel.SetActive(true);
        fillBar.value = 0;

        float elapsed = 0;

        while (elapsed < displayTime)
        {
            elapsed += Time.deltaTime;
            fillBar.value = elapsed / displayTime;
            yield return null;
        }

        panel.SetActive(false);
    }
}