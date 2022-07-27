using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Main : UI_Panel
{
    enum Buttons
    {
        menu_btn,
        notice_btn,
        //goalAdd_btn,
    }

    enum Texts
    {
        date_txt,
        title_txt,
    }

    enum GameObjects
    {
        GoalList,
    }

    //GameObject goalList;


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Text title = GetText((int)Texts.title_txt);
        title.text = Managers.Player.GetString(Define.NICKNAME)+"님의 목표리스트";

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy")+"."+today.ToString("MM")+"."+today.ToString("dd");

        GameObject menuBtn = GetButton((int)Buttons.menu_btn).gameObject;
        BindEvent(menuBtn, MenuBtnClick, Define.TouchEvent.Touch);

        GameObject noticeBtn = GetButton((int)Buttons.notice_btn).gameObject;
        BindEvent(noticeBtn, (data) => { Managers.UI.ShowPopupUI<UI_Signal>("SignalView", "Signal"); }, Define.TouchEvent.Touch);
    }

    private void Start()
    {
        Init();
    }


    private void MenuBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        Managers.UI.ShowPopupUI<UI_Menu>("MenuView", "Menu");
    }

}
