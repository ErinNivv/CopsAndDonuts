using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PuddleInstantiate : MonoBehaviour
{
    public GameObject[] puddles;
    public Transform[] puddleTransforms;
    public float spawnTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PuddleSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PuddleSpawn()
    {
        while (true)
        {
            int wanted = Random.Range(0, puddleTransforms.Length);
            Vector3 position = puddleTransforms[wanted].position;

            GameObject gameObject = Instantiate(puddles[Random.Range(0,puddles.Length)],position, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
            Destroy(gameObject, 7f);
        }
    }
}
