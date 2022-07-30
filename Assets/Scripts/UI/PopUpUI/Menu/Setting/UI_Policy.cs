using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Policy : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
    }
    // ================================ //

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    void Start()
    {
        Init();
    }
}
