using System.Collections;
using UnityEngine;

public class MemoryUsageCounter : MonoBehaviour
{
    [SerializeField] int size = 25;
    [SerializeField] Rect rect = new Rect(5, 35, 200, 50);

    float memoryUsage;
    WaitForSeconds wait = new WaitForSeconds(1f);

    IEnumerator Start()
    {
        while (true)
        {
            memoryUsage = (float)System.GC.GetTotalMemory(false) / (1024 * 1024);
            yield return wait;
        }
    }

    void OnGUI()
    {
        GUI.depth = 2;
        GUI.color = Color.black;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = size;
        style.fontStyle = FontStyle.Bold;
        GUI.Label(rect, "RAM: " + memoryUsage.ToString("F1") + " MB", style);
    }
}
