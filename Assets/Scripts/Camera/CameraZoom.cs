using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    Camera cam;
    float perspectiveZoomSpeed = 0.5f;
    float orthoZoomSpeed = 0.3f;

    float zoomSpeed = 5f;


    private float Speed = 0.7f;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;

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
        if(evt == Define.TouchEvent.TwoTouch)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //처음 터치한 위치(touchzero.position)에서 이전 프레임에서의 터치 위치와 이번 프레임에서 터치 위치의 차이를 뺌
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (cam.orthographic)
            {
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                //cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 7f, 16f);
            }
            else
            {
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 6f, 16f);
            }

        }
#if UNITY_EDITOR
#else
        //이동
        else if (evt == Define.TouchEvent.Touch)
        {

            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
        }
        //카메라 이동
        else if(evt== Define.TouchEvent.TouchMove || evt==Define.TouchEvent.Press)
        {
            Touch touch = Input.GetTouch(0);

            nowPos = touch.position - touch.deltaPosition;
            movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
            cam.transform.Translate(movePos);

            prePos = touch.position - touch.deltaPosition;
        }
#endif
    }

    void ZoomWheel(Define.EditorEvent evt)
    {
        if (evt != Define.EditorEvent.Wheel)
        {
            return;
        }
        //Debug.Log("Wheel");

        if (cam != null)
        {
            float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
            if (distance != 0)
            {
                if (cam.orthographic)
                {
                    cam.orthographicSize += distance;
                    /*                if (cam.orthographicSize < 1)
                                    {
                                        cam.orthographicSize = 1f;
                                    }
                                    else if(cam.orthographicSize > 10)
                                    {
                                        cam.orthographicSize = 10f;
                                    }*/
                    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 7f, 16f);
                }
                else
                {
                    cam.fieldOfView += distance;
                }

            }
        }


    }

    public void AddAction()
    {
        Managers.Input.TouchAction -= Zoom;
        Managers.Input.TouchAction += Zoom;
    }

    public void RemoveAction()
    {
        Managers.Input.TouchAction -= Zoom;
    }
}
