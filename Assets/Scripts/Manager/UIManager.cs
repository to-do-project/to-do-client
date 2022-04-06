using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI�� sort order �����ϸ� popup ����
public class UIManager 
{
    //���� ui order ����
    int _order = 10;
    //���� ������ ShowPopupUI�� ȣ���ϴ� ������ SetCanvas�� ����
    //���� ���Ҵ� UIManager���� ClosePopupUI ȣ���ϸ鼭 ����

    //stack���� popup UI ����ִ´�
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    UI_Panel panelUI = null;

    //UI �θ� ������Ʈ
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

    //name : prefab �̸�
    //T : script�� ����(UI�� ��ũ��Ʈ
    //gameobject ���� �� name�̶� T ���缭 ������� -> name�� ������ T �̸�����
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        //UI ���� 
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        popupStack.Push(popup);


        go.transform.SetParent(Root.transform);

        return popup;


    }

    //UI �ݱ�
    //�������� ����
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

    //���� �Լ��� �������� ����. �߸��� ������ ����
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

    //��� �˾� �ݱ�
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    //subitem ����
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


    //SceneUI ����
    public T ShowSceneUI<T>(string name = null) where T : UI_Panel
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        //UI ���� 
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        panelUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;


    }


    //���ο� UI ų��
    //���� UI���� ���� ����
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; //canvas�� �ڽ� canvas���� �θ� order�� ������� �ڽ��� order�� ����

        if (sort)
        {
            canvas.sortingOrder = (_order++);
        }
        //���� ��û ���� -> �Ϲ� UI��
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        panelUI = null;
    }
}