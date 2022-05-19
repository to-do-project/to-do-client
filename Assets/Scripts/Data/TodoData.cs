using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Todo
{
    public int index;
    public string todoName;
    public int like;
}

public class TodoData : ILoader<int, Todo>
{
    public List<Todo> todos = new List<Todo>();

    public Dictionary<int, Todo> MakeDict()
    {
        throw new System.NotImplementedException();
    }
}
