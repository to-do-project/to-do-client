using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Notification : UI_Popup
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

    private void CameraSet()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera UIcam = canvas.worldCamera;
        if (UIcam == null)
        {
            Camera cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        else
        {
            Debug.Log($"{UIcam.name}");
        }
    }

    private void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        GameObject backBtn = GetButton((int)Buttons.Back_btn).gameObject;
        BindEvent(backBtn, BackBtnClick, Define.TouchEvent.Touch);

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
        friend = false;
        group = false;
        skill = false;
        announce = false;
    }

    #region ButtonEvents
    public void BackBtnClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
    public void PushBtnClick(PointerEventData data)
    {
        friend = group = skill = announce = !push;
        push = !push;
        ChangeBtnImage();
    }
    public void FriendBtnClick(PointerEventData data)
    {
        friend = !friend;
        CheckPush();
        ChangeBtnImage();
    }
    public void GroupBtnClick(PointerEventData data)
    {
        group = !group;
        CheckPush();
        ChangeBtnImage();
    }
    public void SkillBtnClick(PointerEventData data)
    {
        skill = !skill;
        CheckPush();
        ChangeBtnImage();
    }
    public void AnnounceBtnClick(PointerEventData data)
    {
        announce = !announce;
        CheckPush();
        ChangeBtnImage();
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

    private void Start()
    {
        Init();
    }
}
