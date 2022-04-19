using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI의 sort order 관리하며 popup 관리
public class UIManager
{

    //현재 ui order 저장
    int _order = 10;
    //오더 증가는 ShowPopupUI를 호출하는 곳에서 SetCanvas로 증가
    //오더 감소는 UIManager에서 ClosePopupUI 호출하면서 감소

    //stack으로 popup UI 들고있는다
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    UI_Panel panelUI = null;
    UI_Popup tmpPopup = null;

    //UI 부모 오브젝트
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    //name : prefab 이름
    //T : script와 연관(UI의 스크립트
    //gameobject 만들 때 name이랑 T 맞춰서 만들거임 -> name이 없으면 T 이름으로
    public T ShowPopupUI<T>(string name = null, string folder = null) where T : UI_Popup
    {
        string path;

        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        //UI 생성 
        if (string.IsNullOrEmpty(folder))
        {
            path = name;
        }
        else
        {
            path = folder + "/" + name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{path}");
        T popup = Util.GetOrAddComponent<T>(go);
        popupStack.Push(popup);


        go.transform.SetParent(Root.transform);

        return popup;


    }


    //UI 닫기
    //삭제까지 해줌
    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    //위의 함수의 안정적인 버전. 잘못된 삭제를 방지
    public void ClosePopupUI(UI_Popup popup)
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }
    
    //모든 팝업 닫기
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    //첫번째 UI 제외 모든 팝업 닫기
    public void CloseExceptFirstPopupUI()
    {
        while (popupStack.Count > 1)
        {
            ClosePopupUI();
        }
    }

    public void CLoseExceptLastPopupUI()
    {
        tmpPopup = popupStack.Pop();
        CloseAllPopupUI();
        popupStack.Push(tmpPopup);
        tmpPopup.gameObject.transform.SetParent(Root.transform);
    }


    //subitem 생성
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }


        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Util.GetOrAddComponent<T>(go);

    }


    //SceneUI 띄우기
    public T ShowPanelUI<T>(string name = null) where T : UI_Panel
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        //UI 생성 
        GameObject go = Managers.Resource.Instantiate($"UI/Panel/{name}");
        T PanelUI = Util.GetOrAddComponent<T>(go);
        panelUI = PanelUI;

        go.transform.SetParent(Root.transform);

        return PanelUI;


    }


    //새로운 UI 킬때
    //기존 UI와의 순서 정함
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; //canvas의 자식 canvas들이 부모 order에 관계없이 자신의 order를 가짐

        if (sort)
        {
            canvas.sortingOrder = (_order++);
        }
        //소팅 요청 안함 -> 일반 UI임
        else
        {
            canvas.sortingOrder = 0;
        }
    }


    //Main 씬에서 호출 시 stack count가 0이면 앱종료
    public void CloseAppOrUI(Define.Scene scene)
    {
        if (scene==Define.Scene.Main)
        {

        }
        else if(scene==Define.Scene.Login)
        {
            if (popupStack.Count ==0)
            {
                //Debug.Log("Quit");
                Application.Quit();
            }
            else
            {
                //Debug.Log($"Count : {popupStack.Count}");
                ClosePopupUI();
            }
        }
    }


    public void Clear()
    {
        CloseAllPopupUI();
        panelUI = null;
    }
}
