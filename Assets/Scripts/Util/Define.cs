using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum Scene
    {
        Unknown,
        Login,
        Main,
        Edit,
        Menu,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum TouchEvent
    {
        Touch,
        Press,
        Drag,
        TwoTouch,
    }

    public enum Planet
    {
        EMPTY,
        RED,
        GREEN,
        BLUE,
    }

    public enum SystemEvent 
    {
        Back,
        Home,
    }

    public enum EditorEvent
    {
        Wheel,
    }

}
