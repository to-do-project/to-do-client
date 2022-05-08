using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Color : UI_Popup
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

        setBtns();

        menuScene = FindObjectOfType<MenuScene>();

    }

    private void setBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

        GameObject lightRedBtn = GetButton((int)Buttons.LightRed_btn).gameObject;
        BindEvent(lightRedBtn, LightRedBtnClick, Define.TouchEvent.Touch);

        GameObject yellowBtn = GetButton((int)Buttons.Yellow_btn).gameObject;
        BindEvent(yellowBtn, YellowBtnClick, Define.TouchEvent.Touch);

        GameObject greenBtn = GetButton((int)Buttons.Green_btn).gameObject;
        BindEvent(greenBtn, GreenBtnClick, Define.TouchEvent.Touch);

        GameObject skyBlueBtn = GetButton((int)Buttons.SkyBlue_btn).gameObject;
        BindEvent(skyBlueBtn, SkyBlueBtnClick, Define.TouchEvent.Touch);

        GameObject blueBtn = GetButton((int)Buttons.Blue_btn).gameObject;
        BindEvent(blueBtn, BlueBtnClick, Define.TouchEvent.Touch);

        GameObject lightPurpleBtn = GetButton((int)Buttons.LightPurple_btn).gameObject;
        BindEvent(lightPurpleBtn, LightPurpleBtnClick, Define.TouchEvent.Touch);

        GameObject purpleBtn = GetButton((int)Buttons.Purple_btn).gameObject;
        BindEvent(purpleBtn, PurpleBtnClick, Define.TouchEvent.Touch);

        GameObject pinkBtn = GetButton((int)Buttons.Pink_btn).gameObject;
        BindEvent(pinkBtn, PinkBtnClick, Define.TouchEvent.Touch);

        GameObject grayBtn = GetButton((int)Buttons.Gray_btn).gameObject;
        BindEvent(grayBtn, GrayBtnClick, Define.TouchEvent.Touch);

        GameObject blackBtn = GetButton((int)Buttons.Black_btn).gameObject;
        BindEvent(blackBtn, BlackBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
    public void LightRedBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.LightRed);
    }
    public void YellowBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Yellow);
    }
    public void GreenBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Green);
    }
    public void SkyBlueBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.SkyBlue);
    }
    public void BlueBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Blue);
    }
    public void LightPurpleBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.LightPurple);
    }
    public void PurpleBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Purple);
    }
    public void PinkBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Pink);
    }
    public void GrayBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Gray);
    }
    public void BlackBtnClick(PointerEventData data)
    {
        ColorBtnClick(Colors.Black);
    }
    #endregion

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
