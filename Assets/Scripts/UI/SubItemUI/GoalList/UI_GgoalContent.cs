using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GgoalContent : UI_Base
{

    enum Buttons
    {
        groupCheck_btn,
    }

    enum Texts
    {
        Goal_txt,
        GoalRate_txt,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));


    }

    void Start()
    {
        Init();
    }

    private void GroupCheckClicked(PointerEventData data)
    {
        //그룹목표확인창 띄우기
        //
    }

    public void SetGoalName(string name)
    {

    }

    public void SetGoalRate(string rate)
    {

    }
}
