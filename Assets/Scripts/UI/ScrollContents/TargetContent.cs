using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetContent : MonoBehaviour
{
    [SerializeField]
    Text title;
    long id;

    public void ChangeText(string text)
    {
        title.text = text;
    }

    public void SetId(long id)
    {
        this.id = id;
    }

    public void BtnClicked()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        Managers.Web.SendUniRequest("api/goals/archive/" + id, "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                Managers.Todo.UserTodoInstantiate((uwr) => {
                    Managers.UI.ActiveAllUI();
                    FindObjectOfType<UIDataCamera>().RefreshGoalData();
                    FindObjectOfType<UI_GoalList>().callback.Invoke(uwr);
                    Managers.UI.CloseAllPopupUI();
                });
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(BtnClicked);
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }
}
