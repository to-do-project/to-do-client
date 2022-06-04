using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{
    private Vector3 initMousePos;

    GameObject FalseBtn, CheckBtn, DisableImg;
    GameObject Height;

    //아이템 프리팹 최상위 오브젝트
    GameObject root;

    //이벤트핸들러 구독용
    ItemCollision Colevt;
    FalseBtn_EventHandler Falseevt;
    FixBtn_EventHandler Fixevt;
    Item item;

    private bool isFixed; //false면 편집가능상태 아니면 편집 불가 상태

    private float timer = 0f;
    private float MaxDragTime = 0.3f;

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

            root.transform.position = new Vector3(root.transform.position.x + diffPos.x, root.transform.position.y +
                diffPos.y, root.transform.position.z);
            
            CheckOnGround();

        }

        else
        {
            if (timer >= MaxDragTime)
            {
                timer = 0f;
                ChangeFixState(true);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }


    }

    //행성 위에 있는 지 확인
    private void CheckOnGround()
    {
        int layerMask = 1 << LayerMask.NameToLayer("PlanetGround");

        Vector3 pos = Height.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 100f, layerMask);
        if (hit)
        {
            DisableImg.SetActive(false);
            canFixed = true;
        }
        else
        {
            DisableImg.SetActive(true);
            canFixed = false;
        }
    }



    //아이템 편집 모드로 전환
    private void ChangeFixState(bool change)
    {
        isFixed = !change;
        FalseBtn.SetActive(change);
        CheckBtn.SetActive(change);
        //Height.SetActive(!change);
        DisableImg.SetActive(change);
        if (change)
        {
            Colevt.OnCollisionEvent -= CollisionCheck;
            Colevt.OnCollisionEvent += CollisionCheck;
        }
        else
        {
            Colevt.OnCollisionEvent -= CollisionCheck;

        }
    }

    private void BindChildren()
    {
        root = transform.parent.parent.gameObject;

        CheckBtn = Util.FindChild(root, "Check_btn", true);
        FalseBtn = Util.FindChild(root, "False_btn", true);
        DisableImg = Util.FindChild(gameObject, "disable_img", true);
        Height = Util.FindChild(gameObject, "height", true);

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
            Colevt.OnCollisionEvent -= CollisionCheck;
            Colevt.OnCollisionEvent += CollisionCheck;

            Fixevt.OnClickHandler -= FixItem;
            Fixevt.OnClickHandler += FixItem;

            Falseevt.OnClickHandler -= CancleItem;
            Falseevt.OnClickHandler += CancleItem;

            item.OnItemClickAction -= OnMouseDownEvt;
            item.OnItemClickAction += OnMouseDownEvt;

            item.OnItemDragAction -= OnMouseDragEvt;
            item.OnItemDragAction += OnMouseDragEvt;

        }

        else
        {
            ChangeFixState(false);
            Colevt.OnCollisionEvent -= CollisionCheck;
            Fixevt.OnClickHandler -= FixItem;
            Falseevt.OnClickHandler -= CancleItem;
            item.OnItemClickAction -= OnMouseDownEvt;
            item.OnItemDragAction -= OnMouseDragEvt;

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
        Debug.Log("Cancle Item");
        //Managers.Resource.Destroy(root);
        Managers.Player.RemoveItemList(root);
        Managers.Resource.Destroy(root);
    }

    //아이템 충돌 체크
    void CollisionCheck(Collider2D collision)
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
    }


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
