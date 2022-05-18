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
                if (ComparePassword())
                {
                    //ȸ�� Ż��
                    Debug.Log("ȸ�� Ż��");
                    Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
                }
                else
                {
                    PswdChecktxt.text = "*��й�ȣ�� �߸� �Է��߽��ϴ�.";
                }
            }
        });
    }

    private bool ComparePassword()
    {
        //�н����� üũ
        string playerPassword = "";
        bool result = (Pswdfield.text == playerPassword);
        result = true;
        return result;
    }

    private void Start()
    {
        Init();
    }
}
