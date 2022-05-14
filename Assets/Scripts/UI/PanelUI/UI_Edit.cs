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
        
    }

    void Start()
    {
        Init();    
    }


}
