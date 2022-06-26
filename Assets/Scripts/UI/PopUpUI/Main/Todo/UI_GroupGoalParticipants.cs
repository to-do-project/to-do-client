using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupGoalParticipants : UI_Popup
{
    enum Texts
    {
        date_txt,
        GoalTitle_txt,
        GoalRate_txt,
    }
    enum Buttons
    {
        exit_btn,
        GroupDelete_btn,
    }

    enum Images
    {
        myProfile_img,
    }

    enum GameObjects
    {
        OwnerGoal_content,
        OwnerTodo,
        Content,
        AddTodo,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        Text date = GetText((int)Texts.date_txt);
        DateTime today = DateTime.Now;
        date.text = today.ToString("yyyy") + "." + today.ToString("MM") + "." + today.ToString("dd");

    }

}
