using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Delete : UI_Popup
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

    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, ClosePopupUI, Define.TouchEvent.Touch);

        GameObject nextBtn = GetButton((int)Buttons.Next_btn).gameObject;
        BindEvent(nextBtn, NextBtnClick, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }

    private void NextBtnClick(PointerEventData data)
    {
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
    }

    private bool ComparePassword()
    {
        //�н����� üũ
        string playerPassword = "";
        bool result = (Pswdfield.text == playerPassword);
        result = true;
        return result;
    }
}
