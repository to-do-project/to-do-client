using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Collector : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
    }

    private GameObject content = null;
    GameObject parent;

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        if (content == null)
        {
            content = GameObject.Find("CollectorContent");
        }

        foreach (var tmp in dataContainer.goalList)
        {
            AddTarget(tmp.title, tmp.goalId);
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, (data) => { Managers.Sound.PlayNormalButtonClickSound(); ClosePopupUI(); });
    }

    private void Start()
    {
        Init();
    }

    //API에서 데이터 가져와서 컨텐츠에 집어넣기(최신순 먼저)
    void AddTarget(string title, long id)
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/TargetContent");
        target.transform.SetParent(content.transform, false);

        TargetContent tmp = target.GetComponent<TargetContent>();
        tmp.ChangeText(title);
        tmp.SetId(id);
    }

    public void DeleteTarget(long id)
    {
        foreach(var tmp in dataContainer.goalList)
        {
            if(tmp.goalId == id)
            {
                dataContainer.goalList.Remove(tmp);
            }
        }
        parent.GetComponent<UI_Menu>().ChangeCcount();
    }
}
