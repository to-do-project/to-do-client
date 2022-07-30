using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 알림 설정 UI에 사용되는 스크립트
public class UI_Notification : UI_PopupMenu
{
    // 바인딩 할 자식 오브젝트 이름들
    enum Buttons
    {
        Back_btn,
        Push_btn,
        Friend_btn,
        Group_btn,
        Skill_btn,
        Announce_btn,
    }
    // ================================ //

    // 설정 버튼들
    Button pushBtn, friendBtn, groupBtn, skillBtn, announceBtn;
    // 버튼 토글 스크립트
    ImageSet imageSet;

    // 버튼 토글 변수
    bool push, friend, group, skill, announce;

    // 초기화
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        imageSet = GetComponent<ImageSet>();

        SetToggles();
    }

    // 버튼 이벤트 설정
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // 전체 알림 버튼
        pushBtn = GetButton((int)Buttons.Push_btn);
        BindEvent(pushBtn.gameObject, PushBtnClick, Define.TouchEvent.Touch);

        // 친구 알림 버튼
        friendBtn = GetButton((int)Buttons.Friend_btn);
        BindEvent(friendBtn.gameObject, FriendBtnClick, Define.TouchEvent.Touch);

        // 그룹 알림 버튼
        groupBtn = GetButton((int)Buttons.Group_btn);
        BindEvent(groupBtn.gameObject, GroupBtnClick, Define.TouchEvent.Touch);

        // 기능 알림 버튼
        skillBtn = GetButton((int)Buttons.Skill_btn);
        BindEvent(skillBtn.gameObject, SkillBtnClick, Define.TouchEvent.Touch);

        // 공지사항 알림 버튼
        announceBtn = GetButton((int)Buttons.Announce_btn);
        BindEvent(announceBtn.gameObject, AnnounceBtnClick, Define.TouchEvent.Touch);
    }

    // 토글 초기화
    void SetToggles()
    {
        push = false;
        friend = dataContainer.settings.friendFlag == 1;
        group = dataContainer.settings.groupFlag == 1;
        skill = dataContainer.settings.settingFlag == 1;
        announce = dataContainer.settings.noticeFlag == 1;
        CheckPush();
        ChangeBtnImage();
    }

    #region ButtonEvents
    public void PushBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        SendSettingData("ALL", () =>
        {
            friend = group = skill = announce = !push;
            push = !push;
            ChangeBtnImage();
        });
    }
    public void FriendBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        SendSettingData("FRIEND", () =>
        {
            friend = !friend;
            dataContainer.settings.friendFlag = friend ? 1 : 0;
            CheckPush();
            ChangeBtnImage();
        });
    }
    public void GroupBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        SendSettingData("GROUP", () =>
        {
            group = !group;
            dataContainer.settings.groupFlag = group ? 1 : 0;
            CheckPush();
            ChangeBtnImage();
        });
    }
    public void SkillBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        SendSettingData("SETTING", () =>
        {
            skill = !skill;
            dataContainer.settings.settingFlag = skill ? 1 : 0;
            CheckPush();
            ChangeBtnImage();
        });
    }
    public void AnnounceBtnClick(PointerEventData data)
    {
        Managers.Sound.PlayNormalButtonClickSound();
        SendSettingData("NOTICE", () =>
        {
            announce = !announce;
            dataContainer.settings.noticeFlag = announce ? 1 : 0;
            CheckPush();
            ChangeBtnImage();
        });
    }
    #endregion

    // 전체 알림 on off 여부 판단
    void CheckPush()
    {
        push = (friend && group && skill && announce);
    }

    // 토글에 따라서 이미지 변경
    void ChangeBtnImage()
    {
        pushBtn.image.sprite = imageSet.GetImage(push);
        friendBtn.image.sprite = imageSet.GetImage(friend);
        groupBtn.image.sprite = imageSet.GetImage(group);
        skillBtn.image.sprite = imageSet.GetImage(skill);
        announceBtn.image.sprite = imageSet.GetImage(announce);
    }

    // 알림 정보 데이터 웹 통신
    // data = 알림 종류
    // action = 통신 완료 후 실행 함수
    void SendSettingData(string data, System.Action action)
    {
        // 웹 통신 헤더 값
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // 웹 통신 request 값
        RequestSetting req = new RequestSetting
        {
            flag = data,
            deviceToken = GetDeviceToken()
        };

        // 알림 정보 데이터 웹 통신
        Managers.Web.SendUniRequest("api/alarms", "PATCH", req, (uwr) => {
            // 웹 응답 json 데이터를 유니티 데이터로 전환
            Response<ResponseSetting> response = JsonUtility.FromJson<Response<ResponseSetting>>(uwr.downloadHandler.text);

            // 웹 통신 성공 시
            if (response.isSuccess)
            {
                // 받아온 함수 실행
                action.Invoke();
            }
            // 토큰 오류 시
            else if (response.code == 6000)
            {
                // 토큰 재발급 후 실행
                Managers.Player.SendTokenRequest(() => {
                    string[] hN = { Define.JWT_ACCESS_TOKEN,
                                    "User-Id" };
                    string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                                    Managers.Player.GetString(Define.USER_ID) };

                    RequestSetting req = new RequestSetting
                    {
                        flag = data,
                        deviceToken = GetDeviceToken()
                    };

                    Managers.Web.SendUniRequest("api/alarms", "PATCH", req, (uwr) => {
                        Response<ResponseSetting> response = JsonUtility.FromJson<Response<ResponseSetting>>(uwr.downloadHandler.text);
                        if (response.isSuccess)
                        {
                            action.Invoke();
                        }
                        else if (response.code == 6000)
                        {
                            Debug.Log("토큰 재발급 실패");
                        }
                        else
                        {
                            Debug.Log(response.message);
                        }
                    }, hN, hV);
                });
            }
            // 기타 오류 시
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void Start()
    {
        Init();
    }
}
