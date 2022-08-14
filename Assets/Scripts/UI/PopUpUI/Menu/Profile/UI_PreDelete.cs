using UnityEngine.UI;

public class UI_PreDelete : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Next_btn,
        Back_btn,
    }
    // ================================ //

    // ���
    const string pathName = "Menu/Profile";

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();
    }

    // ��ư �̺�Ʈ ����
    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // �Ϸ� ��ư
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
