using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomUIContextMenu
{
    [MenuItem("GameObject/UI/Custom/Button", false, 10)]
    static void CreateCustomButton(MenuCommand menuCommand)
    {
        CreateCustomPrefab(menuCommand, "Button/Button");
    }

    [MenuItem("GameObject/UI/Custom/TMP", false, 11)]
    static void CreateCustomText(MenuCommand menuCommand)
    {
        CreateCustomPrefab(menuCommand, "Text/TMP");
    }

    static void CreateCustomPrefab(MenuCommand menuCommand, string path)
    {
        GameObject prefab = (GameObject)Resources.Load(path);

        if (prefab == null)
        {
            Debug.LogError("Prefab not found. Make sure the prefab path is correct.");
            return;
        }

        // Create an instance of the prefab
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
        Selection.activeObject = instance;

        // Get the object that was right-clicked
        GameObject parent = menuCommand.context as GameObject;

        if (parent != null)
        {
            // Parent the button to the object that was right-clicked
            instance.transform.SetParent(parent.transform, false);
        }
        else
        {
            // If no object was right-clicked, create a new canvas and parent the button to it
            Canvas canvas = Object.FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            // Parent the button to the canvas
            instance.transform.SetParent(canvas.transform, false);
        }

        // Set default position for the button (optional)
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
