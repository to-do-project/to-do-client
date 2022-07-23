using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;



/// <summary>
/// 인벤토리 가로 스크롤뷰
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
        //유저가 가지고 있는 아이템 정보 긁어오기
        //아이템과 일치하는 프리팹을 스크롤뷰에 생성

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
