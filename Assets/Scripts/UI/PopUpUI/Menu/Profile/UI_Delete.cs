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
            //비밀번호가 맞는지 확인한 후
            //회원 탈퇴 체크 후
            //다음 팝업 생성
            string password = Pswdfield.text;
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                if (ComparePassword())
                {
                    //회원 탈퇴
                    Debug.Log("회원 탈퇴");
                    Managers.UI.ShowPopupUI<UI_DeleteCheck>("DeleteCheckView", pathName);
                }
                else
                {
                    PswdChecktxt.text = "*비밀번호를 잘못 입력했습니다.";
                }
            }
        });
    }

    private bool ComparePassword()
    {
        //패스워드 체크
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
