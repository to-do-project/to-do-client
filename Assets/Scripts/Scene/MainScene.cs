using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


[System.Serializable]
public class ResponseDailyResult
{
    public int total_exp;
    public int total_point;
}

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
        Managers.Input.SystemTouchAction -= OnBackTouched;
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;

        //Debug.Log("MainScene");
        Managers.UI.ShowPanelUI<UI_Main>("MainView");


        PlanetCamera = GameObject.Find("PlanetCamera").GetComponent<Camera>();
        //Debug.Log(PlanetCamera.name);

        innerCallback -= PlanetSetting;
        innerCallback += PlanetSetting;

        /*Managers.Input.TouchAction -= EnterArrayMode;
        Managers.Input.TouchAction += EnterArrayMode;*/

        Managers.Input.SystemTouchAction -= OnBackTouched;
        Managers.Input.SystemTouchAction += OnBackTouched;


        PlanetSetting();
        //Managers.Player.SendTokenRequest(innerCallback);


        if (PlayerPrefs.GetInt("FirstStart") == 0 && Managers.Player.GetInt(Define.EXP)== 0)
        {

            PlayerPrefs.SetInt("FirstStart", 1);
            DateTime date = DateTime.Now;
            int sum = date.Year * 430 + date.Month * 32 + date.Day;
            Managers.Player.SetInt(Define.DATETIME, sum);
        }
        else
        {
            if (CalcDate())
            {
                //Managers.UI.ShowPopupUI<UI_DailySettleView>();
                Debug.Log("정산하기");
                Managers.Web.SendGetRequest("api/users/result", null, (uwr) => {
                    Response<ResponseDailyResult> res = JsonUtility.FromJson<Response<ResponseDailyResult>>(uwr.downloadHandler.text);

                    Debug.Log(Managers.Player.GetInt(Define.EXP) + " " + Managers.Player.GetInt(Define.POINT));
                    Debug.Log(res.result.total_point + " " + res.result.total_exp);

                    if (res.isSuccess)
                    {
                        Debug.Log(Managers.Player.GetInt(Define.EXP) + " " + Managers.Player.GetInt(Define.POINT));
                        if(Managers.Player.GetInt(Define.EXP)==-1 || Managers.Player.GetInt(Define.POINT) == -1){
                            return;
                        }

                        UI_DailySettleView ui = Managers.UI.ShowPopupUI<UI_DailySettleView>("DaliySettleView", "Main");
                        ui.Setting(res.result.total_point, res.result.total_exp);

                    }
                    else
                    {
                        if (res.code == 6023)
                        {

                        }
                    }

                }, Managers.Player.GetHeader(), Managers.Player.GetHeaderValue());


            }

        }

        //test 코드
        Managers.Sound.PlayBGMSound();
    }

    void Awake()
    {
        Init();
    }
    
    

/*    //배치모드로 진입
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

    }*/

    void OnBackTouched(Define.SystemEvent evt)
    {
        if (evt != Define.SystemEvent.Back)
        {
            return;
        }

        Debug.Log("BackTouch");

        Managers.UI.CloseAppOrUI();
        Managers.UI.ActivePanelUI();

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
                Debug.Log("하루 이상 지났습니다 : "+ Managers.Player.GetInt(Define.DATETIME)+" "+sum);
                Managers.Player.SetInt(Define.DATETIME, sum);
                return true;
            }
            Managers.Player.SetInt(Define.DATETIME, sum);       // 현재 시간 데이터 저장
            return false;
        }
    }
}
