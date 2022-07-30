using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DeleteCheck : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        End_btn,
    }
    // ================================ //

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        // 안드로이드 뒤로가기 기능 제거(팝업 삭제를 방지하기 위해)
        Managers.Input.SystemTouchAction -= Managers.Input.SystemTouchAction;
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 확인 버튼 || 앱 종료
        SetBtn((int)Buttons.End_btn, (data) => { Application.Quit(); });
    }

    void Start()
    {
        Init();
    }
}
