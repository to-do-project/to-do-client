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

    string pathName = "Menu/Profile";
    const string profileName = "Art/UI/Profile/Profile_Color_3x";

    Image image;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        Bind<Image>(typeof(Images));

        image = GetImage((int)Images.Profile_image);
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

    /*              Manager를 이용한 코드(Test를 위해 주석화)
    void Logout()
    {
        RequestLogout request = new RequestLogout();
        request.deviceToken = "testingtesting";
        Managers.Web.SendPostRequest<Response<string>>("log-out", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if(response.code == 1000)
            {
                Debug.Log(response.result);
            } else
            {
                Debug.Log(response.message);
            }
        });
    }
    */

    void Logout()
    {
        List<string> hN = new List<string>();
        List<string> hV = new List<string>();

        hN.Add("User-Id");
        hV.Add(Testing.instance.UserId);

        RequestLogout request = new RequestLogout();
        request.deviceToken = Testing.instance.DeviceToken;

        Testing.instance.Webbing("log-out", "POST", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
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
        image.sprite = Resources.LoadAll<Sprite>(profileName)[index];
    }

    void Start()
    {
        Init();
    }
}
