using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GoalAdd : UI_Base
{
    
    public override void Init()
    {
        
        BindEvent(this.gameObject, AddBtnClick, Define.TouchEvent.Touch);

    }

    void Start()
    {
        Init();
    }


    private void AddBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_GoalCreate>("GoalCreateView","Main");
    }
}
