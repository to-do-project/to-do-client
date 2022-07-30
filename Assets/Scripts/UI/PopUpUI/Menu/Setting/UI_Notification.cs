using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// �˸� ���� UI�� ���Ǵ� ��ũ��Ʈ
public class UI_Notification : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
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

    // ���� ��ư��
    Button pushBtn, friendBtn, groupBtn, skillBtn, announceBtn;
    // ��ư ��� ��ũ��Ʈ
    ImageSet imageSet;

    // ��ư ��� ����
    bool push, friend, group, skill, announce;

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        CameraSet();

        SetBtns();

        imageSet = GetComponent<ImageSet>();

        SetToggles();
    }

    // ��ư �̺�Ʈ ����
    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // ��ü �˸� ��ư
        pushBtn = GetButton((int)Buttons.Push_btn);
        BindEvent(pushBtn.gameObject, PushBtnClick, Define.TouchEvent.Touch);

        // ģ�� �˸� ��ư
        friendBtn = GetButton((int)Buttons.Friend_btn);
        BindEvent(friendBtn.gameObject, FriendBtnClick, Define.TouchEvent.Touch);

        // �׷� �˸� ��ư
        groupBtn = GetButton((int)Buttons.Group_btn);
        BindEvent(groupBtn.gameObject, GroupBtnClick, Define.TouchEvent.Touch);

        // ��� �˸� ��ư
        skillBtn = GetButton((int)Buttons.Skill_btn);
        BindEvent(skillBtn.gameObject, SkillBtnClick, Define.TouchEvent.Touch);

        // �������� �˸� ��ư
        announceBtn = GetButton((int)Buttons.Announce_btn);
        BindEvent(announceBtn.gameObject, AnnounceBtnClick, Define.TouchEvent.Touch);
    }

    // ��� �ʱ�ȭ
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

    // ��ü �˸� on off ���� �Ǵ�
    void CheckPush()
    {
        push = (friend && group && skill && announce);
    }

    // ��ۿ� ���� �̹��� ����
    void ChangeBtnImage()
    {
        pushBtn.image.sprite = imageSet.GetImage(push);
        friendBtn.image.sprite = imageSet.GetImage(friend);
        groupBtn.image.sprite = imageSet.GetImage(group);
        skillBtn.image.sprite = imageSet.GetImage(skill);
        announceBtn.image.sprite = imageSet.GetImage(announce);
    }

    // �˸� ���� ������ �� ���
    // data = �˸� ����
    // action = ��� �Ϸ� �� ���� �Լ�
    void SendSettingData(string data, System.Action action)
    {
        // �� ��� ��� ��
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // �� ��� request ��
        RequestSetting req = new RequestSetting
        {
            flag = data,
            deviceToken = GetDeviceToken()
        };

        // �˸� ���� ������ �� ���
        Managers.Web.SendUniRequest("api/alarms", "PATCH", req, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<ResponseSetting> response = JsonUtility.FromJson<Response<ResponseSetting>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.isSuccess)
            {
                // �޾ƿ� �Լ� ����
                action.Invoke();
            }
            // ��ū ���� ��
            else if (response.code == 6000)
            {
                // ��ū ��߱� �� ����
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
                            Debug.Log("��ū ��߱� ����");
                        }
                        else
                        {
                            Debug.Log(response.message);
                        }
                    }, hN, hV);
                });
            }
            // ��Ÿ ���� ��
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
