using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerSelectionPanel : MonoBehaviour
{
    [Header("UI References")]
    public Image characterPortrait;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterDescText;
    public Image lockedOverlay;
    public TextMeshProUGUI lockedText;
    public Image takenIndicator;
    public Image panelBorder;
    

    //[Header("Arrows (optional)")]
    //public Image leftArrow;
    //public Image rightArrow;

    private Coroutine takenShakeCoroutine;

    private void Awake()
    {
        if (lockedOverlay != null) lockedOverlay.gameObject.SetActive(false);
        if (takenIndicator != null) takenIndicator.gameObject.SetActive(false);
    }


    public void UpdateDisplay(CharacterData data, bool isTaken)
    {
        if (characterPortrait != null && data.portrait != null)
            characterPortrait.sprite = data.portrait;

        if (characterNameText != null)
            characterNameText.text = data.characterName;

        if (characterDescText != null)
            characterDescText.text = data.description;

        if (takenIndicator != null)
            takenIndicator.gameObject.SetActive(isTaken);

        if (panelBorder != null)
            panelBorder.color = data.panelColor;
    }

    public void ShowLocked(CharacterData data)
    {
        if (lockedOverlay != null)
        {
            Color c = panelBorder != null ? panelBorder.color : data.panelColor;
            c.a = 0.7f;
            lockedOverlay.color = c;
            lockedOverlay.gameObject.SetActive(true);
        }
        if (lockedText != null) lockedText.text = "LOCKED IN\n" + data.characterName;
    }

    public void PlayTakenFeedback()
    {
        if (takenShakeCoroutine != null) StopCoroutine(takenShakeCoroutine);
        takenShakeCoroutine = StartCoroutine(ShakePanel());
    }

    private IEnumerator ShakePanel()
    {
        Vector3 original = transform.localPosition;
        float duration = 0.3f;
        float magnitude = 8f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = original.x + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, original.y, original.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = original;
    }
}