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

        SetBtn((int)Buttons.Logout_btn, (data) => { Debug.Log("*Clicked Button* Logout"); });

        SetBtn((int)Buttons.Delete_btn, (data) => { Managers.UI.ShowPopupUI<UI_Delete>("DeleteView", pathName); });
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
