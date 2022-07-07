using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    //PlayerPref¿¡ ¾µ Key °ª
    public const string JWT_ACCESS_TOKEN = "Jwt-Access-Token";
    public const string JWT_REFRESH_TOKEN = "Jwt-Refresh-Token";
    public const string EMAIL = "email";
    public const string DEVICETOKEN = "deviceToken";
    public const string NICKNAME = "nickname";
    public const string CHARACTER_COLOR = "characterColor";
    public const string PLANET_ID = "planetId";
    public const string PLANET_LEVEL = "planetLevel";
    public const string PLANET_COLOR = "planetColor";
    public const string USER_ID = "userId";
    public const string PROFILE_COLOR = "profileColor";
    public const string CHARACTER_ITEM = "characterItem";
    public const string POINT = "point";
    public const string LEVEL = "level";
    public const string AGE = "age";
    public const string AVG_COMPLETE = "avgGoalCompleteRate";
    public const string GET_GOOD = "getFavoriteCount";
    public const string GIVE_GOOD = "putFavoriteCount";
    public const string DATETIME = "DateTime";
    public const string MISSION_STATUS = "missionStatus";
    public const string EXP = "exp";
    public const string PASSWORD = "password";
    public const string USEDPOINT = "usedPoint";

    public enum Scene
    {
        Unknown,
        Login,
        Main,
        Edit,
        Menu,
        Start,
        Loading,
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
