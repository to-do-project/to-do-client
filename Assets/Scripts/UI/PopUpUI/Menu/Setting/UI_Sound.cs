using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Sound : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Mission_btn,
        Bgm_btn,
        Sfx_btn,
    }

    enum Sliders
    {
        Bgm_slider,
        Sfx_slider,
    }

    enum GameObjects
    {
        Bgm_toggle,
        Sfx_toggle,
    }

    ImageSet imageSet;

    Button missionBtn, bgmBtn, sfxBtn;
    Slider bgmSlider, sfxSlider;
    GameObject bgmToggle, sfxToggle;
    bool mission, bgm, sfx, clicked = false;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        imageSet = GetComponent<ImageSet>();

        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(GameObjects));

        bgmSlider = Get<Slider>((int)Sliders.Bgm_slider);
        bgmSlider.onValueChanged.AddListener(delegate { ValueChangeBGM(); });

        sfxSlider = Get<Slider>((int)Sliders.Sfx_slider);
        sfxSlider.onValueChanged.AddListener(delegate { ValueChangeSFX(); });

        bgmToggle = Get<GameObject>((int)GameObjects.Bgm_toggle);
        sfxToggle = Get<GameObject>((int)GameObjects.Sfx_toggle);

        mission = bgm = sfx = true; //값 가져오기(임시로 true 설정)


        if (PlayerPrefs.HasKey(Define.SYSTEM_MISSION)) mission = (Managers.Player.GetInt(Define.SYSTEM_MISSION) == 1);
        else mission = true;

        //버튼 이미지 초기화
        missionBtn.image.sprite = imageSet.GetImage(mission);

        bgmBtn.image.sprite = imageSet.GetImage(bgm);
        bgmToggle.SetActive(bgm);

        sfxBtn.image.sprite = imageSet.GetImage(sfx);
        sfxToggle.SetActive(sfx);
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        missionBtn = GetButton((int)Buttons.Mission_btn);
        BindEvent(missionBtn.gameObject, MissionBtnClick, Define.TouchEvent.Touch);

        bgmBtn = GetButton((int)Buttons.Bgm_btn);
        BindEvent(bgmBtn.gameObject, BgmBtnClick, Define.TouchEvent.Touch);

        sfxBtn = GetButton((int)Buttons.Sfx_btn);
        BindEvent(sfxBtn.gameObject, SfxBtnClick, Define.TouchEvent.Touch);
    }

    #region ButtonEvents
    public void MissionBtnClick(PointerEventData data)
    {
        if (clicked) return;
        clicked = true;
        mission = !mission;

        MissionWeb();
    }

    void MissionWeb()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        int status = 0;
        if (mission) status = 1;
        else status = 0;

        Managers.Web.SendUniRequest("api/setting/operator-mission/" + status, "PATCH", null, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.isSuccess)
            {
                missionBtn.image.sprite = imageSet.GetImage(mission);
            }
            else if (response.code == 6000)
            {
                Managers.Player.SendTokenRequest(MissionWeb);
            }
            else
            {
                mission = !mission;
                Debug.Log(response.message);
            }
            clicked = false;
        }, hN, hV);
    }
    public void BgmBtnClick(PointerEventData data)
    {
        bgm = !bgm;
        bgmBtn.image.sprite = imageSet.GetImage(bgm);
        bgmToggle.SetActive(bgm);
    }
    public void SfxBtnClick(PointerEventData data)
    {
        sfx = !sfx;
        sfxBtn.image.sprite = imageSet.GetImage(sfx);
        sfxToggle.SetActive(sfx);
    }
    #endregion

    public void ValueChangeBGM()
    {
        //BGM 소리 값 전달
        Debug.Log($"BGM = {bgmSlider.value}");
    }

    public void ValueChangeSFX()
    {
        //SFX 소리 값 전달
        Debug.Log($"SFX = {sfxSlider.value}");
    }

    private void Start()
    {
        Init();
    }
}
