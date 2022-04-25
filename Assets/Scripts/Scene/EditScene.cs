using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditScene : BaseScene
{
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Edit;
    }


    void Update()
    {

    }
}
