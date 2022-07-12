using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class ResponseInven
{
    public int totalInventoryItemCount;
    public List<InventroyList> inventoryList;
}
[System.Serializable]
public class InventroyList
{
    public string itemCode;
    public int totalCount;
    public int placedCount;
    public int remainingCount;
}

public class UI_Edit : UI_Panel
{

    enum Toggles
    {
        plant_toggle,
        road_toggle,
        rock_toggle,
        etc_toggle,
    }

    enum Buttons
    {
        editDone_btn,
        editCancle_btn,
    }

    enum GameObjects 
    {
        Content,
        ContentArea,
    }

    enum Texts
    {
        invenCount_txt,
    }

    EditScene edit;
    GameObject contentRoot;
    GameObject contentArea;

    Action<UnityWebRequest> callback;
    Response<ResponseInven> res;

    Action innerCallback;

    List<InventroyList> invenList;
    Text invenCountTxt;

    public override void Init()
    {
        base.Init();


        callback -= ResponseAction;
        callback += ResponseAction;

        innerCallback -= SendInvenListRequest;
        innerCallback += SendInvenListRequest;

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        edit = FindObjectOfType<EditScene>();

        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        contentRoot = Get<GameObject>((int)GameObjects.Content);
        contentArea = Get<GameObject>((int)GameObjects.ContentArea);

        invenCountTxt = GetText((int)Texts.invenCount_txt);

        GameObject editDoneBtn = GetButton((int)Buttons.editDone_btn).gameObject;
        GameObject editCancleBtn = GetButton((int)Buttons.editCancle_btn).gameObject;

        Toggle plant = Get<Toggle>((int)Toggles.plant_toggle);
        Toggle rock = Get<Toggle>((int)Toggles.rock_toggle);
        Toggle road = Get<Toggle>((int)Toggles.road_toggle);
        Toggle etc = Get<Toggle>((int)Toggles.etc_toggle);

        edit.category = "plant";
        /*        Debug.Log(Managers.Player.GetHeaderValue()[1]);
                Debug.Log(PlayerPrefs.GetString(Define.USER_ID));*/
        SendInvenListRequest();

        plant.onValueChanged.AddListener((bool bOn) =>
        {
            if (bOn)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                edit.category = "plant";
                SendInvenListRequest();
            }
        });

        road.onValueChanged.AddListener((bool bOn) =>
        {
            if (bOn)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                edit.category = "road";
                SendInvenListRequest();
            }
        });

        rock.onValueChanged.AddListener((bool bOn) =>
        {
            if (bOn)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                edit.category = "stone";
                SendInvenListRequest();
            }
        });

        etc.onValueChanged.AddListener((bool bOn) =>
        {
            if (bOn)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                edit.category = "etc";
                SendInvenListRequest();
            }
        });


        BindEvent(editCancleBtn, EditCancleBtnClick, Define.TouchEvent.Touch);
        BindEvent(editDoneBtn, EditDoneBtnClick, Define.TouchEvent.Touch);

        Managers.Player.UIaction -= ScrollViewDownAction;
        Managers.Player.UIaction += ScrollViewDownAction;
    }

    void Start()
    {
        Init();    
    }

    void EditDoneBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayPopupSound();
        Managers.UI.ShowPopupUI<UI_DoneEdit>("DoneEditView", "Edit");

    }

    void EditCancleBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayPopupSound();
        Managers.UI.ShowPopupUI<UI_ExitEdit>("ExitEditView", "Edit");

    }

    //인벤 조회 API 날리기
    private void SendInvenListRequest()
    {
        res = new Response<ResponseInven>();
        Managers.Web.SendGetRequest("api/inventory/planet-items/", edit.category, callback, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<ResponseInven>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                if (res.code == 1000)
                {

                    invenCountTxt.text = res.result.totalInventoryItemCount.ToString() + "/" + "100";
                    invenList = res.result.inventoryList;
                    ClearScrollView();

                    for (int i = 0; i < invenList.Count; i++)
                    {

                        //Debug.Log(invenList[i].itemCode);
                        UI_EditItem go = Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, invenList[i].itemCode);
                        go.SetText(invenList[i].totalCount, invenList[i].remainingCount, invenList[i].placedCount);
                    }


                }
            }

            else
            {
                Debug.Log(res.message);
                //token 재발급
                if (res.code == 6000 || res.code == 6004 || res.code == 6006)
                {
                    Managers.Player.SendTokenRequest(innerCallback);
                }
                else if(res.code == 6028){
                    
                }
            }

            res = null;
        }
    }

    private void ClearScrollView()
    {


        Transform[] childList = contentRoot.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for(int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != contentRoot)
                {
                    Managers.Resource.Destroy(childList[i].gameObject);
                }
            }
        }

        
    }


    private void ScrollViewDownAction(bool down)
    {

        if (contentArea != null)
        {
            if (down)
            {
                Vector3 pos = new Vector3(contentArea.transform.localPosition.x, -2256f, contentArea.transform.localPosition.z); ;

                contentArea.transform.localPosition = pos;
                
            }
            else
            {
                Vector3 pos = new Vector3(contentArea.transform.localPosition.x, -1520f, contentArea.transform.localPosition.z); ;

                contentArea.transform.localPosition = pos;
            }
        }

    }

    
}
