using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    GameObject _player = null;

    void Start()
    {

    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
        else if (_mode == Define.CameraMode.NormalView)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;

                // Check if the touch is over a UI element
                int _index = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) ? 0 : -1;
                // if (EventSystem.current.IsPointerOverGameObject(_index))
                // {
                //     // Touch is on a UI element, so do nothing
                //     return;
                // }

                // Check if the touch is over a UI element
                if (true == IsTouchOverSpecificUI(touchPosition))
                {
                    // Touch is on a specific UI element, so do nothing
                    return;
                }

                // Log or process the touch position here if needed
                Debug.Log("Touch Position: " + touchPosition);

                // Convert touch position to a ray
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.name);

                    if(hit.transform.TryGetComponent<LineTouch>(out LineTouch lineTouch))
                    {
                        lineTouch.LineTouched();
                    }
                }
            }
        }
    }

    private bool IsTouchOverSpecificUI(Vector2 touchPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            // Check if the hit UI element is of a specific type
            if (result.gameObject.GetComponent<Button>() != null ||
                result.gameObject.GetComponent<Dropdown>() != null ||
                result.gameObject.GetComponent<Toggle>() != null ||
                result.gameObject.GetComponent<Slider>() != null)
            {
                return true; // Touch is over a specific UI element that should be ignored
            }
        }
        return false; // Touch is not over any specific UI element
    }
}
