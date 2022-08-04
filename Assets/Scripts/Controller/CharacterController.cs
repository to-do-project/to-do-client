using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    enum Faces
    {
        original,
        smile,
        surprise,
        sad,
        angry

    }

    GameObject[] faces = new GameObject[5];
    AnimationController animController;

    void Start()
    {
        Init();
    }

    void Init()
    {

        Bind();
        animController = Util.GetOrAddComponent<AnimationController>(gameObject);

    }

    void Bind()
    {
        string[] names = Enum.GetNames(typeof(Faces));
        
        for(int i = 0; i < names.Length; i++)
        {
            faces[i] = Util.FindChild(gameObject, names[i], true);
        }

    }


    private void OnMouseDown()
    {
        Debug.Log("Mouse donw on character");
        animController.PlayJump();
    }

    public void ChangeFace(int prePercent) 
    {
        if (prePercent >= 90)
        {

        }
        else if (prePercent >=41 && prePercent <=89)
        {

        }
        else if(prePercent <= 40 && prePercent!=0)
        {

        }
        else if (prePercent == 0)
        {

        }

    }

    //가출하기
    public void RunAway() 
    {

    }

    
}
