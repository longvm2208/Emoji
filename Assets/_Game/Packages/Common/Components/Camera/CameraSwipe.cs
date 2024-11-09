using UnityEngine;
using UnityEngine.EventSystems;

public class CameraSwipe : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector2 maxBounds;
    [SerializeField] Vector2 minBounds;

    [SerializeField]
    Transform cameraTransform;

    bool isDragging = false;
    Vector2 previousPosition;
    EventSystem currentEventSystem;

    void Awake()
    {
        currentEventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began &&
                !currentEventSystem.IsPointerOverGameObject(touch.fingerId))
            {
                previousPosition = Input.mousePosition;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) &&
                !currentEventSystem.IsPointerOverGameObject())
            {
                previousPosition = Input.mousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        if (isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;
            Vector2 mouseDelta = currentPosition - previousPosition;
            Vector3 movePosition = -speed * Time.deltaTime * mouseDelta;
            Vector3 newPosition = cameraTransform.position + movePosition;
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
            cameraTransform.position = newPosition;
            previousPosition = currentPosition;
        }
    }
}