using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditScene : BaseScene
{
    //GameObject Planet;
    //GameObject EditItem,EditItem1, EditItem2;

    public string category;

    public override void Clear()
    {
        //Managers.UI.Clear();
        //throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Edit;

        Managers.UI.ShowPanelUI<UI_Edit>("EditView");
        

        Managers.Input.SystemTouchAction -= OnBackTouched;
        Managers.Input.SystemTouchAction += OnBackTouched;

        Managers.Player.ChangeItemModeList(SceneType);


    }

    private void Awake()
    {
        Init();
    }


    void OnBackTouched(Define.SystemEvent evt)
    {
        Managers.UI.ShowPopupUI<UI_ExitEdit>("ExitEditView","Edit");
    }
}
