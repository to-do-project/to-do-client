using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{
    private Vector3 initMousePos;
    void Init()
    {

    }
    void OnMouseDown()
    {
        initMousePos = Input.mousePosition;
        initMousePos.z = 0f;
        initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
    }

    void OnMouseDrag()
    {
        Debug.Log("Drag Event");
        /*        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, 0f);
        */
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = 0f;
        worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);

        Vector3 diffPos = worldPoint - initMousePos;
        diffPos.z = 0;

        initMousePos = Input.mousePosition;
        initMousePos.z = 0f;
        initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);

        transform.position = new Vector3(transform.position.x + diffPos.x, transform.position.y + diffPos.y, transform.position.z);


    }

    /*    void Init()
        {
            EventTrigger trigger = Util.GetOrAddComponent<EventTrigger>(gameObject);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;

            entry.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }*/
}
