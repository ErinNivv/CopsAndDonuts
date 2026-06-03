using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetPointChanger : MonoBehaviour
{
    public GameObject[] targetPoints;
    [SerializeField] private float activeTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TargetChange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TargetChange()
    {
        if (targetPoints == null || targetPoints.Length == 0)
        {
            yield break;
        }

        int lastIndex = -1;

        while (true)
        {
            int wanted;
            do
            {
                wanted = Random.Range(0, targetPoints.Length);
            }
            while (wanted == lastIndex && targetPoints.Length > 1);

            lastIndex = wanted;

            GameObject target = targetPoints[wanted];

            if (target != null)
            {
                target.SetActive(true);
                yield return new WaitForSeconds(activeTime);
                target.SetActive(false);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
