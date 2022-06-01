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

    EditScene edit;

    public override void Init()
    {
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (UIcam != cam)
        {
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }

        edit = FindObjectOfType<EditScene>();

        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        GameObject editDoneBtn = GetButton((int)Buttons.editDone_btn).gameObject;
        GameObject editCancleBtn = GetButton((int)Buttons.editCancle_btn).gameObject;

        Toggle plant = Get<Toggle>((int)Toggles.plant_toggle);
        Toggle rock = Get<Toggle>((int)Toggles.rock_toggle);
        Toggle road = Get<Toggle>((int)Toggles.road_toggle);
        Toggle etc = Get<Toggle>((int)Toggles.etc_toggle);

        plant.onValueChanged.AddListener((bool bOn) =>
        {
            if (plant.isOn)
            {
                edit.category = "plant";
            }
        });

        road.onValueChanged.AddListener((bool bOn) =>
        {
            if (road.isOn)
            {
                edit.category = "road";
            }
        });

        rock.onValueChanged.AddListener((bool bOn) =>
        {
            if (rock.isOn)
            {
                edit.category = "stone";
            }
        });

        etc.onValueChanged.AddListener((bool bOn) =>
        {
            if (etc.isOn)
            {
                edit.category = "etc";
            }
        });


        BindEvent(editCancleBtn, EditCancleBtnClick, Define.TouchEvent.Touch);
        BindEvent(editDoneBtn, EditDoneBtnClick, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();    
    }

    void EditDoneBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_DoneEdit>("DoneEditView", "Edit");

    }

    void EditCancleBtnClick(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_ExitEdit>("ExitEditView", "Edit");

    }

}
