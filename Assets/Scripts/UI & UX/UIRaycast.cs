using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycast : MonoBehaviour
{
    private Camera _camera;

    void Start() {
        _camera = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Create a ray from the center of the screen
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI"))) {
                // Send a click event to the UI element that was hit
                ExecuteEvents.Execute(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
