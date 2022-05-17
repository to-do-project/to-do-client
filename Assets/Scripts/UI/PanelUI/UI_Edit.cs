using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Edit : UI_Panel
{

    enum Toggles
    {
        plant_toggle,
        road_toggle,
        rock_toggle,
        etc_toggle,
    }

    enum Buttons
    {
        editDone_btn,
        editCancle_btn,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        GameObject editDoneBtn = GetButton((int)Buttons.editDone_btn).gameObject;
        GameObject editCancleBtn = GetButton((int)Buttons.editCancle_btn).gameObject;


        BindEvent(editCancleBtn, EditCancleBtnClick, Define.TouchEvent.Touch);
        BindEvent(editDoneBtn, EditDoneBtnClick, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();    
    }

    void EditDoneBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_ExitEdit>("ExitEditView", "Edit");
    }

    void EditCancleBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_DoneEdit>("DoneEditView", "Edit");

    }
}
