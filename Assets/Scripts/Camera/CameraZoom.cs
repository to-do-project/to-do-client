using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        Invoke("Init", 0.5f);
        //Init();
    }

    void Init()
    {
        Managers.Input.WheelAction -= ZoomWheel;
        Managers.Input.WheelAction += ZoomWheel;

        Managers.Input.TouchAction -= Zoom;
        Managers.Input.TouchAction += Zoom;
        
        cam = this.GetComponent<Camera>();
        cam.transform.position = new Vector3(0, 0, -7);

        if (!cam.orthographic)
        {
            cam.fieldOfView = 23;
        }
        else
        {

            cam.orthographicSize = 11f;
        }


    }


    void Zoom(Define.TouchEvent evt)
    {
        if (!string.Equals("Edit", Managers.Scene.CurrentSceneName()))
        {
            //Debug.Log("Edit scene 내부임");
            return;
        }

        if (evt == Define.TouchEvent.TwoTouch)
        {

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

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

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    return;
                }
            }

        }


#if UNITY_EDITOR
#else
        
        //이동
        else if (evt == Define.TouchEvent.Touch)
        {

                    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            return;
        }

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {

                Touch touch = Input.GetTouch(0);


                if (touch.phase == TouchPhase.Began)
                {
                    prePos = touch.position - touch.deltaPosition;
                }

            }
        }
        //카메라 이동
        else if(evt== Define.TouchEvent.TouchMove || evt==Define.TouchEvent.Press)
        {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            return;
        }

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {


                Touch touch = Input.GetTouch(0);

                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                cam.transform.Translate(movePos);

                prePos = touch.position - touch.deltaPosition;
            }
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
