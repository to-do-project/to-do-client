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


        PlanetSetting();
        //Managers.Player.SendTokenRequest(innerCallback);

        //test 코드
        Managers.Sound.PlayBGMSound();
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

    bool CalcDate() // 마지막 접속과 비교해 새 접속이거나 하루 이상 지났으면 true, 아니면 false 반환
    {
        DateTime date = DateTime.Now;
        int sum = date.Year * 430 + date.Month * 32 + date.Day; // yy-mm-dd를 int값으로 병합
        if (PlayerPrefs.HasKey(Define.DATETIME) == false)       // 저장된 데이터가 없으면(새 접속이면)
        {
            Debug.Log("새로운 접속");
            Managers.Player.SetInt(Define.DATETIME, sum);
            return true;    // true 반환 (웹 통신 후 point 획득했을 시 팝업창 띄우기?)
        }
        else
        {
            if (Managers.Player.GetInt(Define.DATETIME) < sum)  // 저장된 데이터와 현재 시간 비교
            {
                Debug.Log("하루 이상 지났습니다");
                return true;
            }
            Managers.Player.SetInt(Define.DATETIME, sum);       // 현재 시간 데이터 저장
            return false;
        }
    }
}
