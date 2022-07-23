using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Notification : UI_PopupMenu
{
    enum Buttons
    {
        Back_btn,
        Push_btn,
        Friend_btn,
        Group_btn,
        Skill_btn,
        Announce_btn,
    }

    Button pushBtn, friendBtn, groupBtn, skillBtn, announceBtn;
    ImageSet imageSet;
    bool push, friend, group, skill, announce;

    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        imageSet = GetComponent<ImageSet>();

        SetToggles();
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        pushBtn = GetButton((int)Buttons.Push_btn);
        BindEvent(pushBtn.gameObject, PushBtnClick, Define.TouchEvent.Touch);

        friendBtn = GetButton((int)Buttons.Friend_btn);
        BindEvent(friendBtn.gameObject, FriendBtnClick, Define.TouchEvent.Touch);

        groupBtn = GetButton((int)Buttons.Group_btn);
        BindEvent(groupBtn.gameObject, GroupBtnClick, Define.TouchEvent.Touch);

        skillBtn = GetButton((int)Buttons.Skill_btn);
        BindEvent(skillBtn.gameObject, SkillBtnClick, Define.TouchEvent.Touch);

        announceBtn = GetButton((int)Buttons.Announce_btn);
        BindEvent(announceBtn.gameObject, AnnounceBtnClick, Define.TouchEvent.Touch);
    }

    private void SetToggles()
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

    private void CheckPush()
    {
        push = (friend && group && skill && announce);
    }

    private void ChangeBtnImage()
    {
        pushBtn.image.sprite = imageSet.GetImage(push);
        friendBtn.image.sprite = imageSet.GetImage(friend);
        groupBtn.image.sprite = imageSet.GetImage(group);
        skillBtn.image.sprite = imageSet.GetImage(skill);
        announceBtn.image.sprite = imageSet.GetImage(announce);
    }

    void SendSettingData(string data, System.Action action)
    {
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
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    private void Start()
    {
        Init();
    }
}
