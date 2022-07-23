using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;



/// <summary>
/// �κ��丮 ���� ��ũ�Ѻ�
/// </summary>
public class UI_EditScroll : UI_Base, IBeginDragHandler, IEndDragHandler
{



    enum GameObjects
    {
        Content,
    }

    GameObject contentRoot;


    public override void Init()
    {
        //������ ������ �ִ� ������ ���� �ܾ����
        //�����۰� ��ġ�ϴ� �������� ��ũ�Ѻ信 ����

        Bind<GameObject>(typeof(GameObjects));
        contentRoot = Get<GameObject>((int)GameObjects.Content);

        BindEvent(this.gameObject, DragEvnet, Define.TouchEvent.Drag);
    }


    void Start()
    {
        Init();
    }

    public void DragEvnet(PointerEventData data)
    {
        //Debug.Log("Drag!!");
        GameObject cam = GameObject.Find("ZoomCam");
        cam.GetComponent<CameraZoom>().RemoveAction();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag!!");
        GameObject cam = GameObject.Find("ZoomCam");
        cam.GetComponent<CameraZoom>().RemoveAction();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Drag!!");
        GameObject cam = GameObject.Find("ZoomCam");
        cam.GetComponent<CameraZoom>().AddAction();
    }
}
