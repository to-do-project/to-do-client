using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Application : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
    }
    // ================================ //

    public override void Init() // 초기화
    {
        base.Init();

        CameraSet(); // 카메라 설정 (상속)

        SetBtns();
    }

    void SetBtns() // 버튼 이벤트 설정
    {
        Bind<Button>(typeof(Buttons)); // 버튼 바인드

        // 뒤로가기 버튼 || 클릭음 재생, 현재 UI 삭제
        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });
    }

    void Start()
    {
        Init();
    }
}
