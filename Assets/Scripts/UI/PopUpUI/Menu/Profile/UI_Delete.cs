using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Delete : UI_PopupMenu
{
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

    string pathName = "Menu/Profile";

    InputField Pswdfield;
    Text PswdChecktxt;

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

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Next_btn, (data) => {
            //��й�ȣ�� �´��� Ȯ���� ��
            //ȸ�� Ż�� üũ ��
            //���� �˾� ����
            string password = Pswdfield.text;
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                ComparePassword();
            }
        });
    }

    private void ComparePassword()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestDelete req = new RequestDelete();
        req.password = Pswdfield.text;

        Managers.Web.SendUniRequest("api/user", "DELETE", req, (data) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Debug.Log("ȸ�� Ż��");
                PlayerPrefs.DeleteAll();
                Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
            }
            else
            {
                Debug.Log(response.message);
                PswdChecktxt.text = "*��й�ȣ�� �߸� �Է��߽��ϴ�.";
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}
