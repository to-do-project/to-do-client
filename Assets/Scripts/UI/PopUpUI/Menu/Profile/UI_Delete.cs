using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Delete : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Next_btn,
        Back_btn,
    }

    enum Texts
    {
        PswdCheck_txt,
    }

    enum InputFields
    {
        Pswd_inputfield,
    }
    // ================================ //

    InputField Pswdfield; // ��й�ȣ �Է� â
    Text PswdChecktxt;    // �Է� ���� �˸� �ؽ�Ʈ

    // ���
    const string pathName = "Menu/Profile";

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));

        Pswdfield = GetInputfiled((int)InputFields.Pswd_inputfield);

        PswdChecktxt = GetText((int)Texts.PswdCheck_txt);
    }

    // ��ư �̺�Ʈ ����
    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // �Ϸ� ��ư || ���� ���� �� ���
        SetBtn((int)Buttons.Next_btn, (data) => {
            if (string.IsNullOrWhiteSpace(Pswdfield.text) == false)
            {
                Managers.Sound.PlayNormalButtonClickSound();
                DeleteAccount();
            }
        });
    }

    // ���� ���� �� ���
    private void DeleteAccount()
    {
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �� ��� Request ��
        RequestDelete req = new RequestDelete();
        req.password = Pswdfield.text;

        // Debug.Log(req.password);
        // ���� ���� �� ���
        Managers.Web.SendUniRequest("api/user", "DELETE", req, (data) => {
            // Json ���� �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);

            // �� ��� ���� ��
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                // FireBase ��ū ���� �� PlayerPrefs ������ ����
                Managers.FireBase.DeleteToken();
                PlayerPrefs.DeleteAll();

                // DeleteCheckView ����
                Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
            }
            // ��� ��߱� ���� ��
            else if (response.code == 6000)
            {
                // ��ū ��߱�
                Managers.Player.SendTokenRequest(DeleteAccount);
            }
            // �� ��� ���� ��
            else
            {
                Debug.Log(response.message);
                // Debug.Log(response.code);
                // �Է� ���� �ؽ�Ʈ ����
                PswdChecktxt.text = "*��й�ȣ�� �߸� �Է��߽��ϴ�.";
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }
}
