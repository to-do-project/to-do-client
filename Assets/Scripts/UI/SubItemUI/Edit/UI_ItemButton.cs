using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemButton : UI_Base
{
    enum GameObjects
    {
        Circle,
    }

    GameObject circle;

    public override void Init()
    {

        Bind<GameObject>(typeof(GameObjects));
        circle = Get<GameObject>((int)GameObjects.Circle);

    }

    private void Start()
    {
        Init();
    }

    public void ChangeButtonColor(bool unable)
    {
        if (unable)
        {
            SpriteRenderer spr = circle.GetComponent<SpriteRenderer>();
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.5f);


        }
        else
        {
            SpriteRenderer spr = circle.GetComponent<SpriteRenderer>();
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1f);
        }
    }
}
