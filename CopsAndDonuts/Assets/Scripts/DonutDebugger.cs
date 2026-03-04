using UnityEngine;

public class DonutDebugger : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.Log("DONUT WAS DESTROYED: " + gameObject.name);
    }

    void OnDisable()
    {
        Debug.Log("DONUT WAS DISABLED: " + gameObject.name);
    }
}
