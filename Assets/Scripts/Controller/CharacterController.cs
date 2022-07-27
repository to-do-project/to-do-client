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

    void Start()
    {
        Init();
    }

    void Init()
    {

        Bind();

    }

    void Bind()
    {
        string[] names = Enum.GetNames(typeof(Faces));
        
        for(int i = 0; i < names.Length; i++)
        {
            faces[i] = Util.FindChild(gameObject, names[i], true);
        }

    }


    public void ChangeFace() 
    {
    
    }

    //가출하기
    public void RunAway() 
    {

    }
}
