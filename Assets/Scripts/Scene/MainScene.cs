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
                Debug.Log("?????ϱ?");
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

        //test ?ڵ?
        Managers.Sound.PlayBGMSound();
    }

    void Awake()
    {
        Init();
    }
    
    

/*    //??ġ?????? ????
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

    bool CalcDate() // ?????? ???Ӱ? ?????? ?? ?????̰ų? ?Ϸ? ?̻? ???????? true, ?ƴϸ? false ??ȯ
    {
        DateTime date = DateTime.Now;
        int sum = date.Year * 430 + date.Month * 32 + date.Day; // yy-mm-dd?? int?????? ????
        if (PlayerPrefs.HasKey(Define.DATETIME) == false)       // ?????? ?????Ͱ? ??????(?? ?????̸?)
        {
            Debug.Log("???ο? ????");
            Managers.Player.SetInt(Define.DATETIME, sum);
            return true;    // true ??ȯ (?? ???? ?? point ȹ?????? ?? ?˾?â ???????)
        }
        else
        {
            if (Managers.Player.GetInt(Define.DATETIME) < sum)  // ?????? ?????Ϳ? ???? ?ð? ????
            {
                Debug.Log("?Ϸ? ?̻? ???????ϴ? : "+ Managers.Player.GetInt(Define.DATETIME)+" "+sum);
                Managers.Player.SetInt(Define.DATETIME, sum);
                return true;
            }
            Managers.Player.SetInt(Define.DATETIME, sum);       // ???? ?ð? ?????? ????
            return false;
        }
    }
}
