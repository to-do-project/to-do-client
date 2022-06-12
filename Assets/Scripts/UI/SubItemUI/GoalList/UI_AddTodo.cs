using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddTodo : UI_Base
{

    enum Buttons
    {
        todoAdd_btn,
    }

    enum InputFields
    {
        todo_inputfield,
    }



    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));
    }

    void Start()
    {
        Init();
    }

    private void AddBtnClick(PointerEventData data)
    {
        InputField todoName = GetInputfiled((int)InputFields.todo_inputfield);
        
        //todo Ãß°¡ API
    }


}
