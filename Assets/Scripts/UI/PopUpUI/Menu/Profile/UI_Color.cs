using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Color : UI_PopupMenu
{
    protected MenuScene menuScene;

    enum Buttons
    {
        Back_btn,
        LightRed_btn,
        Yellow_btn,
        Green_btn,
        SkyBlue_btn,
        Blue_btn,
        LightPurple_btn,
        Purple_btn,
        Pink_btn,
        Gray_btn,
        Black_btn,
    }

    public enum Colors
    {
        LightRed,
        Yellow,
        Green,
        SkyBlue,
        Blue,
        LightPurple,
        Purple,
        Pink,
        Gray,
        Black,
    }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        menuScene = FindObjectOfType<MenuScene>();

    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.LightRed_btn, (data) => { ColorBtnClick(Colors.LightRed); });

        SetBtn((int)Buttons.Yellow_btn, (data) => { ColorBtnClick(Colors.Yellow); });

        SetBtn((int)Buttons.Green_btn, (data) => { ColorBtnClick(Colors.Green); });

        SetBtn((int)Buttons.SkyBlue_btn, (data) => { ColorBtnClick(Colors.SkyBlue); });

        SetBtn((int)Buttons.Blue_btn, (data) => { ColorBtnClick(Colors.Blue); });

        SetBtn((int)Buttons.LightPurple_btn, (data) => { ColorBtnClick(Colors.LightPurple); });

        SetBtn((int)Buttons.Purple_btn, (data) => { ColorBtnClick(Colors.Purple); });

        SetBtn((int)Buttons.Pink_btn, (data) => { ColorBtnClick(Colors.Pink); });

        SetBtn((int)Buttons.Gray_btn, (data) => { ColorBtnClick(Colors.Gray); });

        SetBtn((int)Buttons.Black_btn, (data) => { ColorBtnClick(Colors.Black); });
    }

    private void ColorBtnClick(Colors color)
    {
        // 바뀐 color정보 전달 후 종료
        Debug.Log(color.ToString());
        Managers.UI.ClosePopupUI();
    }

    void Start()
    {
        Init();
    }
}
