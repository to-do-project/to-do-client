using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Camera cam;
    float perspectiveZoomSpeed = 0.5f;
    void Start()
    {
        Init();
    }

    void Init()
    {
        Managers.Input.TouchAction -= Zoom;
        Managers.Input.TouchAction += Zoom;
        cam = this.GetComponent<Camera>();
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

        cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 80f, 128f);

    }
}
