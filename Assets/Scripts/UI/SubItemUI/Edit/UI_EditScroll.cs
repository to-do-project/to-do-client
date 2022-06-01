using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ResponseInven
{
    public long itemId;
    public int totalCount;
    public int placedCount;
    public int remainingCount;
}

/// <summary>
/// 인벤토리 가로 스크롤뷰
/// </summary>
public class UI_EditScroll : UI_Base
{
    Action<UnityWebRequest> callback;
    Response<List<ResponseInven>> res;

    List<ResponseInven> invenList;


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


        res = new Response<List<ResponseInven>>();
        Managers.Web.SendGetRequest("/api/inventory/planet-items/", "plant",callback,Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());

        Managers.UI.MakeSubItem<UI_EditItem>("Edit",contentRoot.transform,"plant_03");
    }


    void Start()
    {
        Init();
    }

    private void ResponseAction(UnityWebRequest request)
    {
        if (res != null)
        {
            res = JsonUtility.FromJson<Response<List<ResponseInven>>>(request.downloadHandler.text);

            if (res.isSuccess)
            {
                if (res.code == 1000)
                {
                    invenList = res.result;
                    
                    for(int i = 0; i < invenList.Count; i++)
                    {
                        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, res.result[i].itemId.ToString());

                    }
                }
            }

            else
            {
                //token 재발급
                if(res.code==6000 || res.code == 6004 || res.code == 6006)
                {

                }
            }
        }
    }
}
