using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Profile : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Profile_btn,
        NickChange_btn,
        PswdChange_btn,
        Logout_btn,
        Delete_btn,
    }

    enum Images
    {
        Profile_image,
    }

    enum Texts
    {
        Profile_text,
    }

    string pathName = "Menu/Profile";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image profileImage;
    Text profileText;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));
        profileImage = GetImage((int)Images.Profile_image);

        if (PlayerPrefs.HasKey("profileColor"))
        {
            ChangeColor(Managers.Player.GetString("profileColor"));
        }

        Bind<Text>(typeof(Texts));
        profileText = GetText((int)Texts.Profile_text);

        if (PlayerPrefs.HasKey(Define.NICKNAME))
        {
            ChangeNickname(Managers.Player.GetString(Define.NICKNAME));
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Profile_btn, (data) => { Managers.UI.ShowPopupUI<UI_Color>("ColorView", pathName); });

        SetBtn((int)Buttons.NickChange_btn, (data) => { Managers.UI.ShowPopupUI<UI_NickChange>("NickChangeView", pathName); });

        SetBtn((int)Buttons.PswdChange_btn, (data) => { Managers.UI.ShowPopupUI<UI_PswdChange>("PswdChangeView", pathName); });

        SetBtn((int)Buttons.Logout_btn, (data) => { Logout(); });

        SetBtn((int)Buttons.Delete_btn, (data) => { Managers.UI.ShowPopupUI<UI_Delete>("DeleteView", pathName); });
    }

    void Logout()
    {
        string[] hN = { "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.USER_ID) };

        RequestLogout request = new RequestLogout();
        request.deviceToken = Managers.Player.GetString(Define.DEVICETOKEN);

        Managers.Web.SendUniRequest("log-out", "POST", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
                PlayerPrefs.DeleteAll();
                UI_Load.Instance.ToLoad(Define.Scene.Login.ToString());
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    public void ChangeColor(string color)
    {
        int index = 0;
        index = (int)((UI_Color.Colors)System.Enum.Parse(typeof(UI_Color.Colors), color));
        profileImage.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    public void ChangeNickname(string nickname)
    {
        profileText.text = nickname;
    }

    void Start()
    {
        Init();
    }
}
