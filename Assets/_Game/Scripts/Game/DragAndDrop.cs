using UnityEngine;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] UnityEvent onBeginDrag;
    [SerializeField] UnityEvent onEndDrag;

    bool isDragging = false;
    Vector2 offset;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        Debug.Log("down");
        isDragging = true;
        // Calculate the offset between the object's position and the mouse position
        offset = transform.position - GetMouseWorldPosition();
        onBeginDrag?.Invoke();
    }

    void OnMouseUp()
    {
        Debug.Log("up");
        isDragging = false;
        onEndDrag?.Invoke();
    }

    void Update()
    {
        if (isDragging)
        {
            // Move the object to the mouse position, adjusted by the offset
            transform.position = GetMouseWorldPosition() + (Vector3)offset;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePoint = Input.mousePosition;

        // Convert screen position to world position
        mousePoint = mainCamera.ScreenToWorldPoint(mousePoint);

        // Force the z position to 0 (to keep the object on the 2D plane)
        mousePoint.z = 0;

        return mousePoint;
    }
}
