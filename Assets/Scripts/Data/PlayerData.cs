using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public string Jwt_Access_Token;
    public string Jwt_Refresh_Token;
    public string email;
    public string password;
    public string deviceToken;
    public string nickname;
    public string characterColor;
    public string planetId;
    
}

public class PlayerData : ILoader<int, Player>
{
    public List<Player> player = new List<Player>();

    public Dictionary<int, Player> MakeDict()
    {
        throw new System.NotImplementedException();
    }
}
