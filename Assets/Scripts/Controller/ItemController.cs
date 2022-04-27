using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{
    private Vector3 initMousePos;

    GameObject FalseBtn, CheckBtn;

    private bool isFixed;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        isFixed = true;
        BindBtn();
        ChangeEditMode(false);
    }
    void OnMouseDown()
    {
        Debug.Log("mousedown "+isFixed);
        //편집 상태
        if (!isFixed)
        {
            initMousePos = Input.mousePosition;
            initMousePos.z = 0f;
            initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
        }
    }

    void OnMouseDrag()
    {
        Debug.Log("drag "+isFixed);
        //이동 상태
        if (!isFixed)
        {

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

        else
        {
            //isFixed = false;
            ChangeEditMode(true);
            FromInven();
        }

    }


    public void FromInven()
    {
        isFixed = false;
        //Debug.Log("make false");
    }

    public void FromPlanet()
    {
        isFixed = true;
    }

    public void ChangeEditMode(bool change)
    {
        FalseBtn.SetActive(change);
        CheckBtn.SetActive(change);
    }

    private void BindBtn()
    {
        FalseBtn = Util.FindChild(gameObject, "False_btn", true);
        CheckBtn = Util.FindChild(gameObject, "Check_btn", true);

        Util.GetOrAddComponent<Item_EventHandler>(CheckBtn);
    }
 

    private void OnCheckBtnClick(PointerEventData data)
    {
        Debug.Log("Check btn click");
    }

    private void OnFalseBtnClick(PointerEventData data)
    {
        Debug.Log("False btn click");
    }
}
