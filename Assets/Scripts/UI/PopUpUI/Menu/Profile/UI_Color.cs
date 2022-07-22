using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Color : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Back_btn,
        LightRed_btn,
        Yellow_btn,
        Green_btn,
        SkyBlue_btn,
        Blue_btn,
        LightPurple_btn,
        Purple_btn,
        Pink_btn,
        Gray_btn,
        Black_btn,
        Next_btn,
    }

    enum Images
    {
        Check_image,
    }
    // ================================ //

    public enum Colors // ������ �÷� enum��
    {
        LightRed = 0,
        Yellow,
        Green,
        SkyBlue,
        Blue,
        LightPurple,
        Purple,
        Pink,
        Gray,
        Black,
    }

    UI_Profile profile;       // ProfileView ��ũ��Ʈ
    UI_Menu menu;             // MenuView ��ũ��Ʈ
    Transform checkTransform; // üũ ǥ�� ��ġ
    GameObject checkImage;    // üũ ǥ�� �̹���
    GameObject curBtn;        // ���� ��ư
    GameObject nexBtn;        // ���� ��ư

    string selectColor;       // ���õ� �÷� �̸�

    public override void Init() // �ʱ�ȭ
    {
        base.Init();

        CameraSet(); // ī�޶� ����

        SetBtns();

        selectColor = null;
        curBtn = nexBtn = null;

        Bind<GameObject>(typeof(Images)); // �̹��� ���ε�

        checkImage = Get<GameObject>((int)Images.Check_image); // üũ �̹��� ������Ʈ ����
        checkImage.SetActive(false);           // üũ �̹��� ��Ȱ��ȭ
        checkTransform = checkImage.transform; // üũ �̹��� Transform ���ε�

        profile = FindObjectOfType<UI_Profile>();
        menu = FindObjectOfType<UI_Menu>();
    }

    public void ColorBtnEnter(GameObject gameObject) // ��ư�� ��ġ �ν� �� ��ư ũ�� Ȯ��
    {
        if (checkImage.activeSelf) return;
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }

    public void ColorBtnExit(GameObject gameObject) // ��ư�� ��ġ ���� �ν� �� ��ư ũ�� ���
    {
        if (checkImage.activeSelf) return;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void SetBtns() // ��ư �̺�Ʈ ����
    {
        Bind<Button>(typeof(Buttons));

        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        SetBtn((int)Buttons.Next_btn, (data) => { ColorChange(); });

        GameObject btnLR = GetButton((int)Buttons.LightRed_btn).gameObject;
        BindEvent(btnLR, (data) => { ColorBtnClick(Colors.LightRed, btnLR); });

        GameObject btnY = GetButton((int)Buttons.Yellow_btn).gameObject;
        BindEvent(btnY, (data) => { ColorBtnClick(Colors.Yellow, btnY); });

        GameObject btnG = GetButton((int)Buttons.Green_btn).gameObject;
        BindEvent(btnG, (data) => { ColorBtnClick(Colors.Green, btnG); });

        GameObject btnSB = GetButton((int)Buttons.SkyBlue_btn).gameObject;
        BindEvent(btnSB, (data) => { ColorBtnClick(Colors.SkyBlue, btnSB); });

        GameObject btnB = GetButton((int)Buttons.Blue_btn).gameObject;
        BindEvent(btnB, (data) => { ColorBtnClick(Colors.Blue, btnB); });

        GameObject btnLP = GetButton((int)Buttons.LightPurple_btn).gameObject;
        BindEvent(btnLP, (data) => { ColorBtnClick(Colors.LightPurple, btnLP); });

        GameObject btnP = GetButton((int)Buttons.Purple_btn).gameObject;
        BindEvent(btnP, (data) => { ColorBtnClick(Colors.Purple, btnP); });

        GameObject btnPink = GetButton((int)Buttons.Pink_btn).gameObject;
        BindEvent(btnPink, (data) => { ColorBtnClick(Colors.Pink, btnPink); });

        GameObject btnGray = GetButton((int)Buttons.Gray_btn).gameObject;
        BindEvent(btnGray, (data) => { ColorBtnClick(Colors.Gray, btnGray); });

        GameObject btnBlack = GetButton((int)Buttons.Black_btn).gameObject;
        BindEvent(btnBlack, (data) => { ColorBtnClick(Colors.Black, btnBlack); });
    }

    void ColorChange()
    {
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        RequestProfileColor request = new RequestProfileColor();
        request.profileColor = selectColor;

        Managers.Web.SendUniRequest("api/user/profile", "PATCH", request, (uwr) => {
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);
            if (response.code == 1000)
            {
                Debug.Log(response.result);
                Managers.Player.SetString(Define.PROFILE_COLOR, selectColor);
                menu.ChangeColor(selectColor);
                profile.ChangeColor(selectColor);
                Managers.UI.ClosePopupUI();
            }
            else
            {
                Debug.Log(response.message);
            }
        }, hN, hV);
    }

    void ColorBtnClick(Colors color, GameObject target)
    {
        // ��ư�� color���� ���� �� ��ư ũ�� ����
        selectColor = color.ToString();
        if(checkImage.activeSelf == false)
        {
            checkImage.SetActive(true);
        }
        nexBtn = target;
        if(curBtn != null)
        {
            curBtn.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        curBtn = nexBtn;
        curBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        checkTransform.position = curBtn.transform.position;

        Managers.Sound.PlayNormalButtonClickSound();
    }

    void Start()
    {
        Init();
    }
}
