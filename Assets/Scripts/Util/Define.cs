using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    /*//PlayerPref¿¡ ¾µ Key °ª
    public const string JWT_ACCESS_TOKEN = "Jwt_Access_Token";
    public const string JWT_REFRESH_TOKEN = "Jwt-Refresh-Token";
    public const string EMAIL = "email";
    public const string PASSWORD = "password";
    public const string DEVICETOKEN = "deviceToken";
    public const string NICKNAME = "nickname";
    public const string CHARACTER_COLOR = "characterColor";
    public const string PLANET_ID = "planetId";*/

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
        TouchMove,
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
