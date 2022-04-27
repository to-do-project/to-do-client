using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Camera cam;
    float perspectiveZoomSpeed = 0.5f;
    float orthoZoomSpeed = 0.5f;

    float zoomSpeed = 10f;

    void Start()
    {
        Init();
    }

    void Init()
    {
        Managers.Input.WheelAction -= ZoomWheel;
        Managers.Input.WheelAction += ZoomWheel;

        Managers.Input.TouchAction -= Zoom;
        Managers.Input.TouchAction += Zoom;
        cam = this.GetComponent<Camera>();
        if (!cam.orthographic)
        {
            cam.fieldOfView = 23;
        }

    }

    void Zoom(Define.TouchEvent evt)
    {
        if(evt != Define.TouchEvent.TwoTouch)
        {
            return;
        }

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        if (cam.orthographic)
        {
            cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
        }
        else
        {
            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 8f, 23f);
        }

    }

    void ZoomWheel(Define.EditorEvent evt)
    {
        if (evt != Define.EditorEvent.Wheel)
        {
            return;
        }
        //Debug.Log("Wheel");

        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if(distance != 0)
        {
            if (cam.orthographic)
            {
                cam.orthographicSize += distance;
                if (cam.orthographicSize < 1)
                {
                    cam.orthographicSize = 1f;
                }
                else if(cam.orthographicSize > 10)
                {
                    cam.orthographicSize = 10f;
                }
            }
            else
            {
                cam.fieldOfView += distance;
            }

        }

    }
}
