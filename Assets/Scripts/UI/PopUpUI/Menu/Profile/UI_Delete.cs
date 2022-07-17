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

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });

        SetBtn((int)Buttons.Next_btn, (data) => {
            //��й�ȣ�� �´��� Ȯ���� ��
            //ȸ�� Ż�� üũ ��
            //���� �˾� ����
            string password = Pswdfield.text;
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                Managers.Sound.PlayNormalButtonClickSound();
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

        Debug.Log(req.password);

        Managers.Web.SendUniRequest("api/user", "DELETE", req, (data) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Debug.Log("ȸ�� Ż��");
                PlayerPrefs.DeleteAll();
                Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
                Managers.FireBase.DeleteToken();
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(ComparePassword);
            }
            else
            {
                Debug.Log(response.message);
                Debug.Log(response.code);
                PswdChecktxt.text = "*��й�ȣ�� �߸� �Է��߽��ϴ�.";
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}
