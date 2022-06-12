using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{
    private Vector3 initMousePos;

    GameObject FalseBtn, CheckBtn, DisableImg;
    GameObject Height, Coll;

    //아이템 프리팹 최상위 오브젝트
    GameObject root;

    //이벤트핸들러 구독용
    ItemCollision Colevt;
    FalseBtn_EventHandler Falseevt;
    FixBtn_EventHandler Fixevt;
    Item item;

    CameraZoom zoomCam;

    private bool isFixed; //false면 편집가능상태 아니면 편집 불가 상태

    private float timer = 0f;
    private float MaxDragTime = 0.5f;

    private bool canFixed;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        BindChildren();
        ChangeFixState(false);
    }
    //단일 터치
    void OnMouseDownEvt()
    {
        //편집 상태
        if (!isFixed)
        {
            initMousePos = Input.mousePosition;
            initMousePos.z = 0f;
            initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
        }
    }
    //드래그
    void OnMouseDragEvt()
    {
        zoomCam.RemoveAction();

        //Debug.Log("isFixed : " + isFixed);
        //이동 상태
        if (!isFixed)
        {

            //Debug.Log(root.name);
            //Height.SetActive(true);
            timer = 0f;
            //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, root.transform.position.z);

            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = 0f;
            worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);

            Vector3 diffPos = worldPoint - initMousePos;
            diffPos.z = 0f;

            initMousePos = Input.mousePosition;
            initMousePos.z = 0f;
            initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);


            //움직인 위치가 자기자신이면??
/*            Vector3 movePos = new Vector3(root.transform.position.x + diffPos.x, root.transform.position.y + diffPos.y, root.transform.position.z);

            int layerMask = 1 << LayerMask.NameToLayer("Item");
            BoxCollider2D coll = Height.GetComponent<BoxCollider2D>();*/

            //root.transform.position = new Vector3(worldPoint.x, worldPoint.y, root.transform.position.z);
            
            root.transform.position = new Vector3(root.transform.position.x + diffPos.x, root.transform.position.y + diffPos.y, root.transform.position.z);


            CheckOnGround();
            CheckOnItem();

        }

        else
        {
            if (timer >= MaxDragTime)
            {
                timer = 0f;
                if (Managers.Player.CheckItemFixState())
                {
                    ChangeFixState(true);
                }

                initMousePos = Input.mousePosition;
                initMousePos.z = 0f;
                initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);

            }
            else
            {
                timer += Time.deltaTime;
            }
        }


    }

    void OnMouseExitEvt()
    {
        zoomCam.AddAction();

    }

    //행성 위에 있는 지 확인
    private void CheckOnGround()
    {
        int layerMask = 1 << LayerMask.NameToLayer("PlanetGround");

        Vector3 pos = Height.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 100f, layerMask);
        if (hit)
        {
            UI_ItemButton itemBtn = Util.GetOrAddComponent<UI_ItemButton>(CheckBtn);
            itemBtn.ChangeButtonColor(false);
            DisableImg.SetActive(false);
            canFixed = true;
        }
        else
        {
            UI_ItemButton itemBtn = Util.GetOrAddComponent<UI_ItemButton>(CheckBtn);
            itemBtn.ChangeButtonColor(true);
            DisableImg.SetActive(true);
            canFixed = false;
        }

        /*int layerMask = 1 << LayerMask.NameToLayer("PlanetGround");
        BoxCollider2D coll = Height.GetComponent<BoxCollider2D>();

        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, transform.forward, 100f, layerMask);

        Debug.DrawRay(coll.bounds.center + new Vector3(0, 0, -0.1f), transform.forward * 10f, Color.red);

        if (hit)
        {

            DisableImg.SetActive(false);
            canFixed = true;

        }
        else
        {
            DisableImg.SetActive(true);
            canFixed = false;

            //Debug.Log("Didn't hit");
        }*/
    }

    private void CheckOnItem()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Item");
        BoxCollider2D coll = Height.GetComponent<BoxCollider2D>();

        //RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size/2, 0f, transform.forward, 100f, layerMask);
        //Debug.DrawRay(coll.bounds.center, transform.forward * 10f, Color.red);
        RaycastHit2D[] hitArray = Physics2D.BoxCastAll(coll.bounds.center, coll.bounds.size / 2, 0f, transform.forward, 100f, layerMask);

        foreach(RaycastHit2D hit in hitArray)
        {
            if (hit)
            {

                if (hit.collider as BoxCollider2D != coll && Util.FindChild(hit.transform.parent.gameObject, "collider", true) != Coll)
                {
                    if (!hit.collider.name.Equals("collider"))
                    {
                        UI_ItemButton itemBtn = Util.GetOrAddComponent<UI_ItemButton>(CheckBtn);
                        itemBtn.ChangeButtonColor(true);
                        // Debug.Log(hit.transform.parent.gameObject.name);
                        DisableImg.SetActive(true);
                        canFixed = false;
                    }


                }
                else
                {
                    //Debug.Log(hit.transform.parent.gameObject.name);

                }
            }
            else
            {
                UI_ItemButton itemBtn = Util.GetOrAddComponent<UI_ItemButton>(CheckBtn);
                itemBtn.ChangeButtonColor(false);
                DisableImg.SetActive(false);
                canFixed = true;
            }

        }
       
    }



    //아이템 편집 모드로 전환
    //change가 true면 편집 가능상태로
    //false면 편집 불가상태로
    public void ChangeFixState(bool change)
    {
        isFixed = !change;
        FalseBtn.SetActive(change);
        CheckBtn.SetActive(change);
        //Height.SetActive(!change);
        DisableImg.SetActive(change);

        if (Managers.Player.UIaction != null)
        {
            Managers.Player.UIaction.Invoke(change);
        }



    }

    private void BindChildren()
    {
        root = transform.parent.parent.gameObject;

        CheckBtn = Util.FindChild(root, "Check_btn", true);
        FalseBtn = Util.FindChild(root, "False_btn", true);
        DisableImg = Util.FindChild(gameObject, "disable_img", true);
        Height = Util.FindChild(gameObject, "height", true);
        Coll = Util.FindChild(gameObject, "collider", true);

        Fixevt = Util.GetOrAddComponent<FixBtn_EventHandler>(CheckBtn);
        Falseevt = Util.GetOrAddComponent<FalseBtn_EventHandler>(FalseBtn);
        Colevt = Util.GetOrAddComponent<ItemCollision>(Height);
        item = Util.GetOrAddComponent<Item>(transform.parent.gameObject);

    }

    //메인씬-편집씬 전환 시
    public void ChangeMode(Define.Scene type)
    {
        if (type==Define.Scene.Edit)
        {
            zoomCam = GameObject.Find("ZoomCam").GetComponent<CameraZoom>();
            //Colevt.OnCollisionEvent -= CollisionCheck;
            //Colevt.OnCollisionEvent += CollisionCheck;

            Fixevt.OnClickHandler -= FixItem;
            Fixevt.OnClickHandler += FixItem;

            Falseevt.OnClickHandler -= CancleItem;
            Falseevt.OnClickHandler += CancleItem;

            item.OnItemClickAction -= OnMouseDownEvt;
            item.OnItemClickAction += OnMouseDownEvt;

            item.OnItemDragAction -= OnMouseDragEvt;
            item.OnItemDragAction += OnMouseDragEvt;

            item.OnItemExitAction -= OnMouseExitEvt;
            item.OnItemExitAction += OnMouseExitEvt;

        }

        else
        {
            ChangeFixState(false);
            //Colevt.OnCollisionEvent -= CollisionCheck;
            Fixevt.OnClickHandler -= FixItem;
            Falseevt.OnClickHandler -= CancleItem;
            item.OnItemClickAction -= OnMouseDownEvt;
            item.OnItemDragAction -= OnMouseDragEvt;
            item.OnItemExitAction -= OnMouseExitEvt;

        }
    }

    //아이템 확인 버튼 클릭 시
    private void FixItem()
    {

        if (canFixed)
        {
            ChangeFixState(false);
        }
        else
        {
            Debug.Log("Cant Fix Item");
        }


    }

    //아이템 취소 버튼 클릭 시
    private void CancleItem()
    {
        if (Managers.Player.UIaction != null)
        {
            Managers.Player.UIaction.Invoke(false);
        }
        Debug.Log("Cancle Item");
        //Managers.Resource.Destroy(root);
        Managers.Player.RemoveItemList(root);
        Managers.Resource.Destroy(root);


    }

    //아이템 충돌 체크
/*    void CollisionCheck(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if (collision.transform.parent.parent.parent.name.Equals(root.name))
            {
                return;
            }

            DisableImg.SetActive(true);
            canFixed = false;
            //collision.transform.parent.GetComponent<ItemController>().ChangeColor(true);
        }

        else
        {
            DisableImg.SetActive(false);
            canFixed = true;
            //collision.transform.parent.GetComponent<ItemController>().ChangeColor(false);
        }
    }*/


    /*    void CollisionExitCheck(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Item"))
            {
                if (collision.transform.parent.parent.parent.name.Equals(root.name))
                {
                    return;
                }
                else
                {
                    collision.transform.parent.GetComponent<ItemController>().ChangeColor(false);
                }
            }
        }*/

    public void ChangeColor(bool change)
    {
        DisableImg.SetActive(change);
        canFixed = !change;
    }

    public bool GetFixState()
    {
        return isFixed;
    }
}
