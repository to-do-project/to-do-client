using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



/// <summary>
/// 인벤토리 가로 스크롤뷰
/// </summary>
public class UI_EditScroll : UI_Base
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

        /*Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_03");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_04");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "plant_05");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "stone_02");
        Managers.UI.MakeSubItem<UI_EditItem>("Edit", contentRoot.transform, "stone_01");
*/

    }


    void Start()
    {
        Init();
    }



}
