using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EditScroll : UI_Base
{
    enum GameObjects
    {
        Content,
    }

    GameObject root;
    
    public override void Init()
    {
        //유저가 가지고 있는 아이템 정보 긁어오기
        //아이템과 일치하는 프리팹을 스크롤뷰에 생성

        Bind<GameObject>(typeof(GameObjects));
        root = Get<GameObject>((int)GameObjects.Content);

        Managers.UI.MakeSubItem<UI_EditItem>("Edit",root.transform,"portal_00");
    }

    void Start()
    {
        Init();
    }

}
