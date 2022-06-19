using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupGoalCreater : UI_Popup
{
    enum Buttons
    {
        exit_btn,
        GroupDelete_btn,
    }

    enum Texts
    {
        date_txt,
        GoalRate_txt,
        GoalTitle_txt,
        myName_txt,
        myGoatRate_txt,
        myGoalAttend_txt,
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


    }
}
