using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class MainScene : BaseScene
{
    Camera PlanetCamera;
    Action<UnityWebRequest> callback;
    Response<List<ResponseInven>> res;

    Action innerCallback;

    public override void Clear()
    {
        //Managers.UI.Clear();
        //throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;

        Managers.UI.ShowPanelUI<UI_Main>("MainView");



        PlanetCamera = GameObject.Find("PlanetCamera").GetComponent<Camera>();
        Debug.Log(PlanetCamera.name);

        innerCallback -= PlanetSetting;
        innerCallback += PlanetSetting;

        Managers.Input.TouchAction -= EnterArrayMode;
        Managers.Input.TouchAction += EnterArrayMode;

        Managers.Input.SystemTouchAction -= OnBackTouched;
        Managers.Input.SystemTouchAction += OnBackTouched;



        Managers.Player.SendTokenRequest(innerCallback);


    }

    void Awake()
    {
        Init();
    }

    //배치모드로 진입
    void EnterArrayMode(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.Press)
        {
            return;
        }

        bool check = Managers.UI.checkPopupOn();

        if (!check && !EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("touch event");

            Vector3 mousePosition;
#if UNITY_EDITOR
            mousePosition = Input.mousePosition;
#else
        mousePosition = Input.GetTouch(0).position;
#endif
            if (PlanetCamera != null)
            {
                mousePosition = PlanetCamera.ScreenToWorldPoint(mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Planet");

                //Debug.Log(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 100f, layerMask);
                //Debug.DrawRay(mousePosition, PlanetCamera.transform.forward * 100, Color.red, 10f);
                if (hit)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    Managers.Scene.LoadScene(Define.Scene.Edit);


                }
            }

            
        }

        


    }

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Managers.UI.CloseAppOrUI();

    }

    void PlanetSetting()
    {
        Managers.Player.FirstInstantiate();

    }
    

}
