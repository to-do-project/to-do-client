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
        List<string> hN = new List<string>();
        List<string> hV = new List<string>();
        hN.Add("Jwt-Access-Token");
        hN.Add("User-Id");
        hV.Add("Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE2NTA3MDM4MTYsImV4cCI6MTY1MTU2NzgxNn0.dshtPR1lsKm_zmg80rHwEqLjuAjvJaCQpKyd1nPnpIY");
        hV.Add("1");

        Testing.instance.Webbing("api/user", "DELETE", Pswdfield.text, (data) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(data.downloadHandler.text);
            if (response.isSuccess)
            {
                Debug.Log(response.result);
                Debug.Log("ȸ�� Ż��");
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
