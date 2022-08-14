using UnityEngine.UI;

public class UI_PreDelete : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Next_btn,
        Back_btn,
    }
    // ================================ //

    // 경로
    const string pathName = "Menu/Profile";

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
    }

    // 버튼 이벤트 설정
    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 완료 버튼
        SetBtn((int)Buttons.Next_btn, (data) => {
            Managers.UI.ShowPopupUI<UI_Delete>("DeleteView", pathName);
            Managers.Sound.PlayNormalButtonClickSound();
        });
    }

    void Start()
    {
        Init();
    }
}
