using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Collector : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
    }
    // ================================ //

    GameObject content = null; // 컨텐츠 부모 오브젝트 (CollectorContent)
    GameObject parent; // 부모 오브젝트 (MenuView)

    public override void Init()
    {
        base.Init();

        CameraSet(); // 카메라 설정(상속)

        SetBtns();

        if (content == null)
        {
            content = GameObject.Find("CollectorContent"); // 컨텐츠 부모 설정
        }

        foreach (var tmp in dataContainer.goalList)
        {
            AddTarget(tmp.title, tmp.goalId); // 목표 추가
                   // 목표 이름, 목표 id
        }
    }
    public void SetParent(GameObject parent) // 부모 설정
    {
        this.parent = parent;
    }

    public void DeleteTarget(long id) // id >> 삭제할 목표 id || 보관 목표 컨텐츠 삭제
    {
        foreach (var tmp in dataContainer.goalList)
        {
            if (tmp.goalId == id)
            {
                dataContainer.goalList.Remove(tmp); // 데이터 컨테이너에서 삭제
            }
        }
        parent.GetComponent<UI_Menu>().ChangeCcount(); // 메뉴 화면의 목표 카운트 수 재설정
    }

    void AddTarget(string title, long id) // title >> 목표 이름, id >> 추가할 목표 id || 보관 목표 컨텐츠 추가
    {
        GameObject target = Managers.Resource.Instantiate("UI/ScrollContents/TargetContent"); // 타겟 오브젝트 생성
        target.transform.SetParent(content.transform, false);     // 타겟 부모에 컨텐츠 부모 연결

        TargetContent tmp = target.GetComponent<TargetContent>();
        tmp.ChangeText(title); // 타겟 오브젝트의 이름 텍스트 설정
        tmp.SetId(id);         // 타겟 오브젝트의 목표 id 설정
    }

    void SetBtns() // 버튼 이벤트 설정
    {
        Bind<Button>(typeof(Buttons)); // 버튼 바인드

        // 뒤로가기 버튼 || 현재 UI 삭제
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);
    }

    void Start()
    {
        Init();
    }
}
